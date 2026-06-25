using Application.Common.Models;
using MediatR;

namespace Application.Features.Transactions.GetTransactionHistory;

public sealed record GetTransactionHistoryQuery(
    Guid WalletId,
    PaginationRequest Pagination)
    : IRequest<PagedResult<GetTransactionHistoryResponse>>;
