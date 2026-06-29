using Domain.Entities;

namespace Application.Common.Interfaces;

/// <summary>
/// Provides authorized access to wallets belonging to the current user.
/// </summary>
public interface IWalletAccessService
{
    /// <summary>
    /// Gets a wallet and verifies that it belongs to the current authenticated user.
    /// </summary>
    Task<Wallet> GetOwnedWalletAsync(Guid walletId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets all wallets belonging to the current authenticated user.
    /// </summary>
    Task<IReadOnlyList<Wallet>> GetOwnedWalletsAsync(
        CancellationToken cancellationToken);
}
