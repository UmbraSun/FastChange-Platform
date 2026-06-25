using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Mapster;
using MediatR;
using Resources;

namespace Application.Features.Transactions.GetTransactionHistory;

public class GetTransactionHistoryQueryHandler 
    : IRequestHandler<
        GetTransactionHistoryQuery,
        PagedResult<GetTransactionHistoryResponse>>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IWalletRepository _walletRepository;

    public GetTransactionHistoryQueryHandler(
        ITransactionRepository transactionRepository, 
        IWalletRepository walletRepository,
        ICurrentUserService currentUserService)
    {
        _transactionRepository = transactionRepository;
        _walletRepository = walletRepository;
        _currentUserService = currentUserService;
    }

    public async Task<PagedResult<GetTransactionHistoryResponse>> Handle(
        GetTransactionHistoryQuery request,
        CancellationToken cancellationToken)
    {
        var wallet = await _walletRepository.GetByIdAsync(request.WalletId, cancellationToken);
        if(wallet is null)
            throw new BusinessException(Localization.WalletNotFound);

        if (wallet.UserId != _currentUserService.UserId)
            throw new BusinessException(Localization.WalletIsNotAssociatedWithThisUser);

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
