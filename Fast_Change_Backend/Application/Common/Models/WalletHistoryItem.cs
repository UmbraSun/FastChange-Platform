namespace Application.Common.Models;

public sealed record WalletHistoryItem(
    Guid OperationId,
    decimal SignedAmount,
    decimal BalanceAfter,
    string OperationType,
    decimal? ExchangeRate,
    decimal? ReceivedAmount,
    DateTime CreatedAtUtc);
