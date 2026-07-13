using BuildingBlocks.Messaging;
using Confluent.Kafka;
using Infrastructure.Observability;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Resources;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Messaging.Kafka;

public abstract class BaseKafkaConsumer<TEvent> 
    : BackgroundService
{
    private readonly IConsumer<string, string> _consumer;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger _logger;

    protected abstract string Topic { get; }

    protected BaseKafkaConsumer(
        IConsumer<string, string> consumer,
        IServiceScopeFactory scopeFactory,
        ILogger logger)
    {
        _consumer = consumer;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        _consumer.Subscribe(Topic);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var result = _consumer.Consume(stoppingToken);

                var parent = result.Message.Headers
                    .FirstOrDefault(h => h.Key == "traceparent");

                var parentContext = parent != null
                    ? ActivityContext.Parse(
                        Encoding.UTF8.GetString(parent.GetValueBytes()),
                        null)
                    : default;

                using var activity = FastChangeTelemetry.ActivitySource.StartActivity(
                    $"Kafka Consume {typeof(TEvent).Name}",
                    ActivityKind.Consumer,
                    parentContext);

                activity?.SetTag(
                    "messaging.system",
                    "kafka");

                activity?.SetTag(
                    "messaging.destination.name",
                    Topic);

                activity?.SetTag(
                    "messaging.kafka.partition",
                    result.Partition.Value);

                activity?.SetTag(
                    "messaging.kafka.offset",
                    result.Offset.Value);

                var @event = Deserialize(result.Message.Value);

                using var scope = _scopeFactory.CreateScope();

                var handlers =scope.ServiceProvider
                    .GetServices<IIntegrationEventHandler<TEvent>>();

                foreach (var handler in handlers)
                    await handler.HandleAsync(
                        @event,
                        stoppingToken);

                _consumer.Commit(result);
            }
            catch (ConsumeException ex)
            {
                _logger.LogError(ex, "Kafka consume error");
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled Kafka consumer error");
            }
        }

        _consumer.Close();
    }

    private static TEvent Deserialize(
        string value)
    {
        return JsonSerializer.Deserialize<TEvent>(value)
            ?? throw new InvalidOperationException(Localization.UnableToDeserializeKafkaEvent);
    }
}
