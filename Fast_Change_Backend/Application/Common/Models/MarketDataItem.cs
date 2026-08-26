namespace Application.Common.Models;

public sealed record MarketDataItem(
    string Currency,
    string QuoteCurrency,
    decimal Price,
    decimal? PriceChangePercentage24h);
