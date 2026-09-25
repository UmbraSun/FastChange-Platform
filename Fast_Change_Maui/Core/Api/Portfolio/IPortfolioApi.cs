using Core.DTOs.Portfolio;
using Refit;

namespace Core.Api.Portfolio;

/// <summary>
/// Represents the portfolio API interface for interacting with portfolio-related endpoints.
/// </summary>
public interface IPortfolioApi
{
    /// <summary>
    /// Retrieves the portfolio information for the specified currency.
    /// </summary>
    /// <param name="currency"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Get("/api/Portfolio")]
    Task<PortfolioResponseDto> GetPortfolioAsync(string currency = "USD", CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the portfolio performance information for the specified currency.
    /// </summary>
    /// <param name="currency"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Get("/api/Portfolio/performance")]
    Task<PortfolioPerformanceResponseDto> GetPerformanceAsync(string currency = "USD", CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the market overview for the specified currencies and quote currency.
    /// </summary>
    /// <param name="currencies"></param>
    /// <param name="currency"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Get("/api/Portfolio/market")]
    Task<IReadOnlyList<MarketDataItemDto>> GetMarketOverviewAsync(
    [Query(CollectionFormat.Multi)] IReadOnlyCollection<string> currencies, string currency = "USD", CancellationToken cancellationToken = default);
}