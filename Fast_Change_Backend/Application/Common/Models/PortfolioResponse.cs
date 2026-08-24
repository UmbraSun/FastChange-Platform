using Application.Features.Portfolio.GetPortfolio;

namespace Application.Common.Models;

public sealed record PortfolioResponse(
    string Currency,
    decimal TotalBalance,
    decimal Change24h,
    decimal Change24hPercent,
    IReadOnlyList<PortfolioWalletResponse> Wallets);
