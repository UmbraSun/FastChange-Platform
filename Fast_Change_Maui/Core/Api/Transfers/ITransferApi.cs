using Core.DTOs.Transfers;
using Refit;

namespace Core.Api.Transfers;

/// <summary>
/// Represents the API interface for transfer operations, including initiating transfers and searching for transfer recipients.
/// </summary>
public interface ITransferApi
{
    /// <summary>
    /// Initiates a transfer between wallets based on the provided transfer request details.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Post("/api/transfers")]
    Task<TransferResponseDto> TransferAsync(
        [Body] TransferRequestDto request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches for transfer recipients based on the provided query and currency. Returns a list of matching transfer recipients.
    /// </summary>
    /// <param name="query"></param>
    /// <param name="currency"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Get("/api/transfers/recipients")]
    Task<IReadOnlyList<TransferRecipientDto>> SearchRecipientsAsync(
        string query,
        string currency,
        CancellationToken cancellationToken = default);
}