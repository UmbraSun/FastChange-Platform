namespace Application.Features.Portfolio.GetPortfolio;

public sealed record PortfolioResponse(
    string Currency,
    decimal TotalBalance,
    IReadOnlyList<PortfolioWalletResponse> Wallets);

public sealed record PortfolioWalletResponse(
    Guid WalletId,
    string Currency,
    decimal Balance,
    decimal ExchangeRate,
    decimal ConvertedValue);
