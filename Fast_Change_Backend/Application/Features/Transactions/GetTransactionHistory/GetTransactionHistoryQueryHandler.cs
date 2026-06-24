using Application.Common.Interfaces;
using MediatR;

namespace Application.Features.Transactions.GetTransactionHistory;

public class GetTransactionHistoryQueryHandler 
    : IRequestHandler<
        GetTransactionHistoryQuery,
        List<GetTransactionHistoryResponse>>
{
    private readonly ITransactionRepository _transactionRepository;

    public GetTransactionHistoryQueryHandler(
        ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task<List<GetTransactionHistoryResponse>> Handle(
        GetTransactionHistoryQuery request,
        CancellationToken cancellationToken)
    {
        var transactions =
            await _transactionRepository.GetByWalletIdAsync(
                request.WalletId,
                cancellationToken);

        return transactions
            .Select(x => new GetTransactionHistoryResponse(
                x.Id,
                x.Currency,
                x.Amount,
                x.Type,
                x.CreatedAtUtc))
            .ToList();
    }
}
