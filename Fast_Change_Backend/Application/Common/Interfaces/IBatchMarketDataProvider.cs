using Application.Common.Models;

namespace Application.Common.Interfaces;

/// <summary>
/// Provides market data for multiple currencies.
/// </summary>
public interface IBatchMarketDataProvider
{
    /// <summary>
    /// Gets market data for multiple currencies.
    /// </summary>
    /// <param name="currencies"></param>
    /// <param name="quoteCurrency"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IReadOnlyList<MarketDataItem>> GetMarketDataAsync(
        IReadOnlyCollection<string> currencies,
        string quoteCurrency,
        CancellationToken cancellationToken);
}
