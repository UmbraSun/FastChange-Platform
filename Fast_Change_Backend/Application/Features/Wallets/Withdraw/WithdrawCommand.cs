using MediatR;

namespace Application.Features.Wallets.Withdraw;

public sealed record WithdrawCommand(
    Guid WalletId,
    decimal Amount)
    : IRequest<WithdrawResponse>;