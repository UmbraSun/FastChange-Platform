using Application.Common.Models;

namespace Application.Common.Interfaces;

/// <summary>
/// Exchange rate cache interface for storing and retrieving exchange rates.
/// </summary>
public interface IExchangeRateCache
{
    /// <summary>
    /// Gets the exchange rate for the specified currency pair from the cache.
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    Task<ExchangeRate?> GetAsync(string from, string to);

    /// <summary>
    /// Sets the exchange rate in the cache with a specified time-to-live (TTL).
    /// </summary>
    /// <param name="rate"></param>
    /// <param name="ttl"></param>
    /// <returns></returns>
    Task SetAsync(ExchangeRate rate, TimeSpan ttl);
}
