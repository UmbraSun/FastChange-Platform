namespace Application.Common.Models;

public sealed record MarketData(
    string Currency,
    string TargetCurrency,
    decimal Price,
    decimal Change24hPercent,
    DateTime RetrievedAtUtc);
