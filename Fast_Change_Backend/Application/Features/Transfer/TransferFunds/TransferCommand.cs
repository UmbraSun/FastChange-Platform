using MediatR;

namespace Application.Features.Transfer.TransferFunds;

public sealed record TransferCommand(
    Guid FromWalletId,
    Guid ToWalletId,
    decimal Amount)
    : IRequest<TransferResponse>;
