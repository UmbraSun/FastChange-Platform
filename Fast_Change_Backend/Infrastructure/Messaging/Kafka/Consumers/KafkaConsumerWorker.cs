using Application.Common.Interfaces;
using Application.Features.Exchange.Events;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Infrastructure.Messaging.Kafka.Consumers;

public sealed class KafkaConsumerWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<KafkaConsumerWorker> _logger;
    private readonly IConsumer<string, string> _consumer;

    public KafkaConsumerWorker(
        IServiceScopeFactory scopeFactory,
        ILogger<KafkaConsumerWorker> logger,
        IConsumer<string, string> consumer)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _consumer = consumer;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _consumer.Subscribe("exchange-events");

        return Task.Run(() =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var result = _consumer.Consume(stoppingToken);

                    using var scope = _scopeFactory.CreateScope();

                    var handler =
                        scope.ServiceProvider
                            .GetRequiredService<IEventHandler<ExchangeCompletedEvent>>();

                    var @event =
                        JsonSerializer.Deserialize<ExchangeCompletedEvent>(result.Message.Value)!;

                    handler.HandleAsync(@event, stoppingToken).GetAwaiter().GetResult();

                    _consumer.Commit(result);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Kafka consumer error");
                }
            }
        }, stoppingToken);
    }
}
