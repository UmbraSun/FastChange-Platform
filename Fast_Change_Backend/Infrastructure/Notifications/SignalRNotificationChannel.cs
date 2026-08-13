using Application.Common.Interfaces;
using Contracts.Events;
using Contracts.Notifications;

namespace Infrastructure.Notifications;

public sealed class SignalRNotificationChannel
    : ITransactionNotificationChannel
{
    private readonly IWalletRepository _walletRepository;
    private readonly IWalletNotificationService _notificationService;

    public SignalRNotificationChannel(
        IWalletRepository walletRepository,
        IWalletNotificationService notificationService)
    {
        _walletRepository = walletRepository;
        _notificationService = notificationService;
    }

    public async Task NotifyAsync(
        TransactionCompletedEvent @event,
        CancellationToken cancellationToken)
    {
        if (@event.FromWalletId != Guid.Empty)
        {
            var fromWallet = await _walletRepository.GetByIdAsync(
                @event.FromWalletId,
                cancellationToken);

            if (fromWallet is not null)
            {
                await _notificationService.WalletUpdatedAsync(
                    fromWallet.UserId,
                    fromWallet.Id,
                    cancellationToken);
            }
        }

        if (@event.ToWalletId != Guid.Empty &&
            @event.ToWalletId != @event.FromWalletId)
        {
            var toWallet = await _walletRepository.GetByIdAsync(
                @event.ToWalletId,
                cancellationToken);

            if (toWallet is not null)
            {
                await _notificationService.WalletUpdatedAsync(
                    toWallet.UserId,
                    toWallet.Id,
                    cancellationToken);
            }
        }
    }
}
