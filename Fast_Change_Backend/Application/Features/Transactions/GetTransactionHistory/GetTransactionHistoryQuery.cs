using MediatR;

namespace Application.Features.Transactions.GetTransactionHistory;

public sealed record GetTransactionHistoryQuery(
    Guid WalletId)
    : IRequest<List<GetTransactionHistoryResponse>>;
