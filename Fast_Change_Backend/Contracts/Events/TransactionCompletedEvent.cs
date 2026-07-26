using Contracts.Enums;

namespace Contracts.Events;

public sealed record TransactionCompletedEvent(
    Guid OperationId,
    Guid FromWalletId,
    Guid ToWalletId,
    decimal Amount,
    string Currency,
    TransactionType Type,
    decimal? ExchangeRate = null);
