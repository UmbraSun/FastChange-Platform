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
}