using Core.DTOs.Common;
using Core.DTOs.Transactions;

namespace Core.Interfaces;

/// <summary>
/// Represents a service for managing transactions.
/// </summary>
public interface ITransactionService
{
    /// <summary>
    /// Gets a paginated list of transactions for a specific wallet.
    /// </summary>
    /// <param name="walletId"></param>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<PagedResultDto<TransactionDto>> GetTransactionsAsync(Guid walletId, int page = 1, int pageSize = 20, CancellationToken cancellationToken = default);
}
