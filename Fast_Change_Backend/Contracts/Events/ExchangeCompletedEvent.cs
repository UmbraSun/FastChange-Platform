namespace Contracts.Events;

public sealed record ExchangeCompletedEvent(
    Guid OperationId,
    Guid FromWalletId,
    Guid ToWalletId,
    decimal Amount,
    decimal Rate,
    decimal ReceivedAmount);
