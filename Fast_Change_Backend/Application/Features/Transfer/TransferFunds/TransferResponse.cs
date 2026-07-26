namespace Application.Features.Transfer.TransferFunds;

public sealed record TransferResponse(
    Guid OperationId,
    decimal Amount,
    decimal SenderBalance,
    decimal ReceiverBalance);
