using Application.Common.Interfaces;
using Application.Common.Models;
using Mapster;
using MediatR;

namespace Application.Features.Transactions.GetTransactionHistory;

public class GetTransactionHistoryQueryHandler
    : IRequestHandler<
        GetTransactionHistoryQuery,
        PagedResult<GetTransactionHistoryResponse>>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IWalletAccessService _walletAccessService;

    public GetTransactionHistoryQueryHandler(
        ITransactionRepository transactionRepository,
        IWalletAccessService walletAccessService)
    {
        _transactionRepository = transactionRepository;
        _walletAccessService = walletAccessService;
    }

    public async Task<PagedResult<GetTransactionHistoryResponse>> Handle(
        GetTransactionHistoryQuery request,
        CancellationToken cancellationToken)
    {
        await _walletAccessService.GetOwnedWalletAsync(
            request.WalletId,
            cancellationToken);

        var transactions =
           await _transactionRepository.GetByWalletIdAsync(
                request.WalletId,
                request.Pagination.Page,
                request.Pagination.PageSize,
                cancellationToken);

        return new PagedResult<GetTransactionHistoryResponse>(
            transactions.Items.Adapt<List<GetTransactionHistoryResponse>>(),
            transactions.TotalCount,
            request.Pagination.Page,
            request.Pagination.PageSize);
    }
}
