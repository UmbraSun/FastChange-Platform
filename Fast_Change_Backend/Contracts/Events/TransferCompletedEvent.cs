namespace Contracts.Events;

public sealed record TransferCompletedEvent(
    Guid OperationId,
    Guid FromWalletId,
    Guid ToWalletId,
    decimal Amount);
