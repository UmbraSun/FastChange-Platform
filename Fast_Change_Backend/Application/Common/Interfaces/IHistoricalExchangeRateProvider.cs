using Application.Common.Models;

namespace Application.Common.Interfaces;

/// <summary>
/// Represents a provider for historical exchange rates.
/// </summary>
public interface IHistoricalExchangeRateProvider
{
    /// <summary>
    /// Gets the historical exchange rate between two currencies for a specific date.
    /// </summary>
    /// <param name="fromCurrency"></param>
    /// <param name="toCurrency"></param>
    /// <param name="date"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<ExchangeRate> GetRateAsync(
        string fromCurrency,
        string toCurrency,
        DateOnly date,
        CancellationToken cancellationToken);
}
