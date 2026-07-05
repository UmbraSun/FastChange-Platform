using Application.Common.Interfaces;
using Contracts.Events;
using Microsoft.Extensions.Logging;

namespace Infrastructure.ExchangeRates.Events;

public sealed class ExchangeCompletedHandler
{
    private readonly ILogger<ExchangeCompletedHandler> _logger;
    private readonly IWalletNotificationService _notificationService;
    private readonly IWalletHistoryWriter _historyWriter;

    public ExchangeCompletedHandler(
        ILogger<ExchangeCompletedHandler> logger,
        IWalletNotificationService notificationService,
        IWalletHistoryWriter historyWriter)
    {
        _logger = logger;
        _notificationService = notificationService;
        _historyWriter = historyWriter;
    }

    public async Task HandleAsync(
        ExchangeCompletedEvent @event,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Exchange completed: {OperationId}",
            @event.OperationId);

        await _historyWriter.AddExchangeAsync(@event, cancellationToken);

        await _notificationService.WalletUpdatedAsync(
            @event.FromWalletId,
            cancellationToken);

        await _notificationService.WalletUpdatedAsync(
            @event.ToWalletId,
            cancellationToken);
    }
}
