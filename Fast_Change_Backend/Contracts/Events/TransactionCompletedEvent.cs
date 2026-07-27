using Contracts.Enums;

namespace Contracts.Events;

public sealed record TransactionCompletedEvent(
    Guid OperationId,
    Guid FromWalletId,
    Guid ToWalletId,
    decimal Amount,
    decimal? ReceivedAmount,
    string FromCurrency,
    string ToCurrency,
    TransactionType Type,
    decimal? ExchangeRate = null);
