using Core.Api.Users;
using Core.DTOs.Common;
using Core.DTOs.Transactions;
using Core.DTOs.Wallets;
using Core.Interfaces;

namespace Core.Services;

public sealed class TransactionService : ITransactionService
{
    private readonly IWalletApi _walletApi;

    public TransactionService(IWalletApi walletApi)
    {
        _walletApi = walletApi;
    }

    public Task<PagedResultDto<TransactionDto>> GetTransactionsAsync(Guid walletId, int page = 1, int pageSize = 20, CancellationToken cancellationToken = default)
    {
        return _walletApi.GetTransactionsAsync(walletId, page, pageSize, cancellationToken);
    }

    public Task<IReadOnlyList<WalletHistoryItemDto>> GetHistoryAsync(Guid walletId, int take = 5, CancellationToken cancellationToken = default)
    {
        return _walletApi.GetHistoryAsync(walletId, take, cancellationToken);
    }
}