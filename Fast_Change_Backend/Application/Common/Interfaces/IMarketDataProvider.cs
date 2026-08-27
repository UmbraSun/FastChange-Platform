using Application.Common.Models;

namespace Application.Common.Interfaces;

/// <summary>
/// Provides current market data and 24-hour price changes for currencies.
/// </summary>
public interface IMarketDataProvider
{
    /// <summary>
    /// Get market data
    /// </summary>
    /// <param name="currency"></param>
    /// <param name="targetCurrency"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<MarketData> GetAsync(
        string currency,
        string targetCurrency,
        CancellationToken cancellationToken);
}
