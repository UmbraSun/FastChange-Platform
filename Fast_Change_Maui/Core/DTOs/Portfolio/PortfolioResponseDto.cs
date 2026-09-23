namespace Core.DTOs.Portfolio;

public sealed record PortfolioResponseDto(
    string Currency,
    decimal TotalBalance,
    IReadOnlyList<PortfolioWalletResponseDto> Wallets);