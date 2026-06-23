namespace Application.Features.Wallets.GetUserWallets;

public sealed record GetUserWalletsResponse(
    Guid WalletId,
    string Currency,
    decimal Balance);
