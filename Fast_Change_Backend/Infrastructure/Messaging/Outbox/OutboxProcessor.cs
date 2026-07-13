using Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Messaging.Outbox;

public sealed class OutboxProcessor
    : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<OutboxProcessor> _logger;

    public OutboxProcessor(
        IServiceScopeFactory scopeFactory,
        ILogger<OutboxProcessor> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();

                var repository = scope.ServiceProvider
                    .GetRequiredService<IOutboxRepository>();

                var producer = scope.ServiceProvider
                    .GetRequiredService<IKafkaProducer>();

                var messages = await repository.GetUnprocessedAsync(100, stoppingToken);

                foreach (var message in messages)
                {
                    await producer.PublishAsync(
                        message.Topic,
                        message.Id.ToString(),
                        message.Payload,
                        null,
                        stoppingToken);

                    await repository.MarkAsProcessedAsync(
                        message.Id,
                        stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Outbox processing failed.");
            }

            await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
        }
    }
}
