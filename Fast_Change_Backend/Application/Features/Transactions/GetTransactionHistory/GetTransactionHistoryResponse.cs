using Domain.Enums;

namespace Application.Features.Transactions.GetTransactionHistory;

public sealed record GetTransactionHistoryResponse(
    Guid TransactionId,
    string Currency,
    decimal Amount,
    TransactionType Type,
    DateTime CreatedAtUtc);
