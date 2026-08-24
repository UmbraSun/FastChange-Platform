namespace Application.Common.Models;

public sealed record PortfolioChange(
    decimal CurrentValue,
    decimal PreviousValue,
    decimal ChangeAmount,
    decimal ChangePercent);
