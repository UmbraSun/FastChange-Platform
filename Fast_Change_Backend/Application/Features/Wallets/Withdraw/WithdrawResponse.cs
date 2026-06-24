namespace Application.Features.Wallets.Withdraw;

public sealed record WithdrawResponse(
    Guid WalletId,
    decimal Balance);
