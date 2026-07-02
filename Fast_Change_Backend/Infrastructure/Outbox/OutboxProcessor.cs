using Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Infrastructure.Outbox;

public sealed class OutboxProcessor : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<OutboxProcessor> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromSeconds(5);

    public OutboxProcessor(
        IServiceScopeFactory scopeFactory,
        ILogger<OutboxProcessor> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var outboxRepo = scope.ServiceProvider.GetRequiredService<IOutboxRepository>();
                var publisher = scope.ServiceProvider.GetRequiredService<IRabbitMqPublisher>();

                var messages = await outboxRepo.GetUnprocessedAsync(20, stoppingToken);

                foreach (var message in messages)
                {
                    try
                    {
                        var domainEventType = Type.GetType($"Domain.Events.{message.Type}");
                        var domainEvent = JsonSerializer.Deserialize(message.Payload, domainEventType!);

                        await publisher.PublishAsync(
                            exchange: "wallet.events",
                            routingKey: message.Type,
                            message: domainEvent!,
                            cancellationToken: stoppingToken);

                        await outboxRepo.MarkAsProcessedAsync(message.Id, stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to publish outbox message {MessageId}", message.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Outbox processor iteration failed");
            }

            await Task.Delay(_interval, stoppingToken);
        }
    }
}
