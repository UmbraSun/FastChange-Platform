using Core.DTOs.Common;
using Core.DTOs.Transactions;
using Refit;

namespace Core.Api.Users;

/// <summary>
/// Represents the API for wallet-related operations.
/// </summary>
public interface IWalletApi
{
    /// <summary>
    /// Retrieves a paginated list of transactions for a specific wallet.
    /// </summary>
    /// <param name="walletId"></param>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Get("/api/Wallet/{walletId}/transactions")]
    Task<PagedResultDto<TransactionDto>> GetTransactionsAsync(Guid walletId, int page = 1, int pageSize = 20, CancellationToken cancellationToken = default);
}
