using Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Infrastructure.BackgroundServices.Outbox;

public sealed class OutboxDispatcher : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly OutboxDispatcherOptions _options;
    private readonly ILogger<OutboxDispatcher> _logger;

    public OutboxDispatcher(
        IServiceScopeFactory scopeFactory,
        IOptions<OutboxDispatcherOptions> options,
        ILogger<OutboxDispatcher> logger)
    {
        _scopeFactory = scopeFactory;
        _options = options.Value;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await DispatchAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Outbox dispatcher failed.");
            }

            await Task.Delay(
                _options.PollingIntervalMs,
                stoppingToken);
        }
    }

    private async Task DispatchAsync(
        CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();

        var repository =
            scope.ServiceProvider.GetRequiredService<IOutboxRepository>();

        var eventBus =
            scope.ServiceProvider.GetRequiredService<IEventBus>();

        var unitOfWork =
            scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var messages =
            await repository.GetUnprocessedAsync(
                _options.BatchSize,
                cancellationToken);

        foreach (var message in messages)
        {
            try
            {
                await eventBus.PublishAsync(
                    message.Id,
                    message.Topic,
                    message.Key,
                    message.Payload,
                    cancellationToken);

                message.ProcessedOnUtc = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to publish Outbox message {MessageId}",
                    message.Id);
            }
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
