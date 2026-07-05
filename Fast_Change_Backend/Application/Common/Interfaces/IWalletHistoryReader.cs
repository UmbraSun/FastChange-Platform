using Application.Common.Models;

namespace Application.Common.Interfaces;

/// <summary>
/// Wallet history reader interface for retrieving wallet history items.
/// </summary>
public interface IWalletHistoryReader
{
    /// <summary>
    /// Gets the wallet history items for a specific wallet.
    /// </summary>
    /// <param name="walletId"></param>
    /// <param name="take"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IReadOnlyList<WalletHistoryItem>> GetByWalletAsync(
        Guid walletId,
        int take,
        CancellationToken cancellationToken);
}
