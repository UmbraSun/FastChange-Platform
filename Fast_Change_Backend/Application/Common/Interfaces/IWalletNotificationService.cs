namespace Application.Common.Interfaces;

/// <summary>
/// Provides real-time notifications about wallet updates.
/// </summary>
public interface IWalletNotificationService
{
    /// <summary>
    /// Notifies a user that one of their wallets has been updated.
    /// </summary>
    /// <param name="userId">The owner of the updated wallet.</param>
    /// <param name="walletId">The updated wallet.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task WalletUpdatedAsync(
        Guid userId,
        Guid walletId,
        CancellationToken cancellationToken);
}
