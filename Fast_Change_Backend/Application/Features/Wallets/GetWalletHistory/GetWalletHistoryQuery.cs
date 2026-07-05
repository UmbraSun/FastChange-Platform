namespace Application.Features.Wallets.GetWalletHistory;

public sealed record GetWalletHistoryQuery(
    Guid WalletId,
    int Take = 50);
