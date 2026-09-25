using Core.DTOs.Portfolio;

namespace Core.Interfaces;

/// <summary>
/// Represents a service for managing and retrieving portfolio-related data.
/// </summary>
public interface IPortfolioService
{
    /// <summary>
    /// Retrieves the portfolio information for the specified currency.
    /// </summary>
    /// <param name="currency"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<PortfolioResponseDto> GetPortfolioAsync(string currency = "USD", CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the performance metrics of the portfolio for the specified currency.
    /// </summary>
    /// <param name="currency"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<PortfolioPerformanceResponseDto> GetPerformanceAsync(string currency = "USD", CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the market overview for the specified currencies and quote currency.
    /// </summary>
    /// <param name="currencies"></param>
    /// <param name="currency"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IReadOnlyList<MarketDataItemDto>> GetMarketOverviewAsync(IReadOnlyCollection<string> currencies, string currency = "USD", CancellationToken cancellationToken = default);
}