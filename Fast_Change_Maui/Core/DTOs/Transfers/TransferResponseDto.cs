namespace Core.DTOs.Transfers;

public sealed record TransferResponseDto(
    Guid OperationId,
    decimal Amount,
    decimal SenderBalance,
    decimal ReceiverBalance);