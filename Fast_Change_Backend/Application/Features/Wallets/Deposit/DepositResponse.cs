namespace Application.Features.Wallets.Deposit;

public sealed record DepositResponse(
    Guid WalletId,
    decimal NewBalance);
