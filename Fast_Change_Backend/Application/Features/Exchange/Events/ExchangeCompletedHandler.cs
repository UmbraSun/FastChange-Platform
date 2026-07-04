using Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Features.Exchange.Events;

public sealed class ExchangeCompletedHandler
    : IEventHandler<ExchangeCompletedEvent>
{
    private readonly ILogger<ExchangeCompletedHandler> _logger;
    private readonly IWalletNotificationService _notificationService;

    public ExchangeCompletedHandler(
        ILogger<ExchangeCompletedHandler> logger,
        IWalletNotificationService notificationService)
    {
        _logger = logger;
        _notificationService = notificationService;
    }

    public async Task HandleAsync(
        ExchangeCompletedEvent @event,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Exchange completed: {OperationId}",
            @event.OperationId);

        await _notificationService.WalletUpdatedAsync(
            @event.FromWalletId,
            cancellationToken);

        await _notificationService.WalletUpdatedAsync(
            @event.ToWalletId,
            cancellationToken);
    }
}
