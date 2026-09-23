namespace Core.DTOs.Portfolio;

public sealed record PortfolioWalletResponseDto(
    Guid WalletId,
    string Currency,
    decimal Balance,
    decimal ExchangeRate,
    decimal ConvertedValue);