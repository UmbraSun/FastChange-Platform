using Application.Common.Models;

namespace Application.Common.Interfaces;

/// <summary>
/// Represents a cache for storing and retrieving market data.
/// </summary>
public interface IMarketDataCache
{
    /// <summary>
    /// Gets the market data from the cache for the given key.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IReadOnlyList<MarketDataItem>?> GetAsync(string key, CancellationToken cancellationToken);

    /// <summary>
    /// Sets the market data in the cache for the given key with an expiration time.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="data"></param>
    /// <param name="expiration"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task SetAsync(string key,
        IReadOnlyList<MarketDataItem> data,
        TimeSpan expiration,
        CancellationToken cancellationToken);
}
