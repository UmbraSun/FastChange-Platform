namespace Application.Features.Portfolio.GetPortfolioPerformance;

public sealed record PortfolioPerformanceResponse(
    string Currency,
    decimal CurrentValue,
    decimal PreviousValue,
    decimal ChangeAmount,
    decimal ChangePercent);
