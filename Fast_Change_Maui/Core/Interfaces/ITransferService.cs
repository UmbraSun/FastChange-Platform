using Core.DTOs.Transfers;

namespace Core.Interfaces;

/// <summary>
/// Represents a service for handling transfers between wallets and searching for transfer recipients.
/// </summary>
public interface ITransferService
{
    /// <summary>
    /// Transfers a specified amount from one wallet to another.
    /// </summary>
    /// <param name="fromWalletId"></param>
    /// <param name="toWalletId"></param>
    /// <param name="amount"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TransferResponseDto> TransferAsync(
        Guid fromWalletId,
        Guid toWalletId,
        decimal amount,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches for transfer recipients based on a query and currency.
    /// </summary>
    /// <param name="query"></param>
    /// <param name="currency"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IReadOnlyList<TransferRecipientDto>> SearchRecipientsAsync(
        string query,
        string currency,
        CancellationToken cancellationToken = default);
}