using Domain.Entities;

namespace Application.Common.Interfaces;

/// <summary>
/// Wallet repository interface for managing wallet-related operations.
/// </summary>
public interface IWalletRepository
{
    /// <summary>
    /// Gets a wallet by its unique identifier.
    /// </summary>
    /// <param name="walletId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Wallet?> GetByIdAsync(
        Guid walletId,
        CancellationToken cancellationToken);

    /// <summary>
    /// Saves changes to the database.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task SaveChangesAsync(
        CancellationToken cancellationToken);
}
