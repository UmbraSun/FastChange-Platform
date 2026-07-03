using Application.Common.Interfaces;
using Application.Features.Exchange.Events;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Messaging.Kafka.Consumers;

public sealed class BaseKafkaConsumer : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<BaseKafkaConsumer> _logger;
    private readonly IConsumer<string, string> _consumer;

    public BaseKafkaConsumer(
        IServiceScopeFactory scopeFactory,
        ILogger<BaseKafkaConsumer> logger,
        IConsumer<string, string> consumer)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _consumer = consumer;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _consumer.Subscribe("exchange-events");

        return Task.Run(() => ConsumeLoop(stoppingToken), stoppingToken);
    }

    private async Task ConsumeLoop(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var result = _consumer.Consume(stoppingToken);

                var eventId = ExtractEventId(result);

                using var scope = _scopeFactory.CreateScope();

                var store = scope.ServiceProvider.GetRequiredService<IProcessedEventStore>();

                var wasProcessed = await store.TryMarkProcessedAsync(eventId, stoppingToken);

                if (!wasProcessed)
                {
                    _consumer.Commit(result);
                    continue;
                }

                var handler = scope.ServiceProvider
                    .GetRequiredService<IEventHandler<ExchangeCompletedEvent>>();

                var @event = Deserialize(result.Message.Value);

                await handler.HandleAsync(@event, stoppingToken);

                _consumer.Commit(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kafka consumer error");
            }
        }
    }

    private static Guid ExtractEventId(ConsumeResult<string, string> result)
    {
        var header = result.Message.Headers
            .FirstOrDefault(h => h.Key == "event-id")
            ?? throw new Exception("Missing event-id header");

        return Guid.Parse(Encoding.UTF8.GetString(header.GetValueBytes()));
    }

    private static ExchangeCompletedEvent Deserialize(string message)
        => JsonSerializer.Deserialize<ExchangeCompletedEvent>(message)!;
}
