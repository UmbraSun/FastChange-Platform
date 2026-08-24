namespace Application.Common.Models;

public sealed record PortfolioPerformance(
    string Currency,
    decimal CurrentValue,
    decimal PreviousValue,
    decimal ChangeAmount,
    decimal ChangePercent);
