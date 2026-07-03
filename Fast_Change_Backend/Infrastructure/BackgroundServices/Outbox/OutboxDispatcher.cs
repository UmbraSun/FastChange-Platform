using Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.BackgroundServices.Outbox;

public sealed class OutboxDispatcher : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<OutboxDispatcher> _logger;

    public OutboxDispatcher(
        IServiceScopeFactory scopeFactory,
        ILogger<OutboxDispatcher> logger)
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

                var store = scope.ServiceProvider.GetRequiredService<IOutboxStore>();
                var producer = scope.ServiceProvider.GetRequiredService<IKafkaProducer>();

                var messages = await store.GetUnprocessedAsync(50, stoppingToken);

                foreach (var message in messages)
                {
                    try
                    {
                        await producer.PublishAsync(
                            message.Topic,
                            message.Key,
                            message.Payload,
                            new Dictionary<string, string>
                            {
                                ["event-id"] = message.Id.ToString()
                            },
                            stoppingToken);

                        await store.MarkAsProcessedAsync(
                            message.Id,
                            DateTime.UtcNow,
                            stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Outbox publish failed {Id}", message.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Outbox dispatcher loop failed");
            }

            await Task.Delay(1000, stoppingToken);
        }
    }
}
