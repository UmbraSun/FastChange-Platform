namespace Core.DTOs.Wallets;

public sealed record WalletHistoryItemDto(
    Guid OperationId,
    Guid WalletId,
    decimal SignedAmount,
    decimal BalanceAfter,
    string OperationType,
    decimal? ExchangeRate,
    decimal? ReceivedAmount,
    DateTime CreatedAtUtc);