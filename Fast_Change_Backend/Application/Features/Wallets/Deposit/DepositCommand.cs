using MediatR;

namespace Application.Features.Wallets.Deposit;

public sealed record DepositCommand(
    Guid WalletId,
    decimal Amount)
    : IRequest<DepositResponse>;
