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

        return Task.Run(async () =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var result = _consumer.Consume(stoppingToken);

                    var header = result.Message.Headers
                        .FirstOrDefault(h => h.Key == "event-id") ?? throw new Exception("Missing event-id header");

                    var eventId = Guid.Parse(
                        Encoding.UTF8.GetString(header.GetValueBytes()));

                    using var scope = _scopeFactory.CreateScope();

                    var handler = scope.ServiceProvider.GetRequiredService<IEventHandler<ExchangeCompletedEvent>>();

                    var store = scope.ServiceProvider.GetRequiredService<IProcessedEventStore>();

                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                    var @event = JsonSerializer.Deserialize<ExchangeCompletedEvent>(result.Message.Value)!;

                    await handler.HandleAsync(@event, stoppingToken);

                    await store.MarkProcessedAsync(eventId, stoppingToken);

                    await unitOfWork.SaveChangesAsync(stoppingToken);

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
