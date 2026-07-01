using Application.Common.Interfaces;
using Application.Features.Exchange.Events;
using Confluent.Kafka;
using Domain.Entities;
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

                    var eventIdHeader =
                        result.Message.Headers
                            .FirstOrDefault(h => h.Key == "event-id");

                    var eventId = Guid.Parse(
                        Encoding.UTF8.GetString(eventIdHeader.Value));

                    using var scope = _scopeFactory.CreateScope();

                    var processedRepo =
                        scope.ServiceProvider.GetRequiredService<IProcessedEventRepository>();

                    var alreadyProcessed =
                        await processedRepo.ExistsAsync(eventId, stoppingToken);

                    if (alreadyProcessed)
                    {
                        _consumer.Commit(result);
                        continue;
                    }

                    var @event =
                        JsonSerializer.Deserialize<ExchangeCompletedEvent>(
                            result.Message.Value)!;

                    var handler =
                        scope.ServiceProvider
                            .GetRequiredService<IEventHandler<ExchangeCompletedEvent>>();

                    await handler.HandleAsync(@event, stoppingToken);

                    await processedRepo.AddAsync(
                        new ProcessedEvent
                        {
                            EventId = eventId,
                            ProcessedAtUtc = DateTime.UtcNow
                        },
                        stoppingToken);

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
