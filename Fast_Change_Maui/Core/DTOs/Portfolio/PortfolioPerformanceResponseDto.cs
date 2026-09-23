namespace Core.DTOs.Portfolio;

public sealed record PortfolioPerformanceResponseDto(
    string Currency,
    decimal CurrentValue,
    decimal PreviousValue,
    decimal ChangeAmount,
    decimal ChangePercent);