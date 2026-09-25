namespace Core.DTOs.Portfolio;

public sealed record MarketDataItemDto(
    string Currency,
    string QuoteCurrency,
    decimal Price,
    decimal? PriceChangePercentage24h);