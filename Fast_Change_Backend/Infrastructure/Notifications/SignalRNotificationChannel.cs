using Application.Common.Interfaces;
using Contracts.Events;
using Contracts.Notifications;

namespace Infrastructure.Notifications;

public sealed class SignalRNotificationChannel
    : ITransactionNotificationChannel
{
    private readonly IWalletNotificationService _notificationService;

    public SignalRNotificationChannel(IWalletNotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public async Task NotifyAsync(
        TransactionCompletedEvent @event,
        CancellationToken cancellationToken)
    {
        await _notificationService.WalletUpdatedAsync(@event.FromWalletId, cancellationToken);
        await _notificationService.WalletUpdatedAsync(@event.ToWalletId, cancellationToken);
    }
}
