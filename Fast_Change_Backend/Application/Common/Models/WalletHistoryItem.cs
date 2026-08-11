namespace Application.Common.Models;

/// <summary>
/// Wallet history item
/// </summary>
/// <param name="OperationId"></param>
/// <param name="SignedAmount"></param>
/// <param name="BalanceAfter"></param>
/// <param name="OperationType"></param>
/// <param name="ExchangeRate"></param>
/// <param name="ReceivedAmount"></param>
/// <param name="CreatedAtUtc"></param>
public sealed record WalletHistoryItem(
    Guid OperationId,
    decimal SignedAmount,
    decimal BalanceAfter,
    string OperationType,
    decimal? ExchangeRate,
    decimal? ReceivedAmount,
    DateTime CreatedAtUtc);
