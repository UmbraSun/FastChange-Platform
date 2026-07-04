using Domain.Entities;

namespace Application.Common.Interfaces;

/// <summary>
/// Wallet notification service interface for sending notifications related to wallet updates.
/// </summary>
public interface IWalletNotificationService
{
    /// <summary>
    /// Updates the wallet and sends a notification to the user about the wallet update.
    /// </summary>
    /// <param name="walletId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task WalletUpdatedAsync(
        Guid walletId,
        CancellationToken cancellationToken);
}
