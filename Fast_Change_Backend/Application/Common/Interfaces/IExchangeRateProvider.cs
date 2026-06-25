namespace Application.Common.Interfaces;

/// <summary>
/// Exchange rate provider interface for retrieving currency exchange rates.
/// </summary>
public interface IExchangeRateProvider
{
    /// <summary>
    /// Get the exchange rate between two currencies asynchronously.
    /// </summary>
    /// <param name="fromCurrency"></param>
    /// <param name="toCurrency"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<decimal> GetRateAsync(
        string fromCurrency,
        string toCurrency,
        CancellationToken cancellationToken);
}
