using Application.Common.Models;

namespace Application.Common.Interfaces;

public interface IExchangeRateCache
{
    Task<ExchangeRate?> GetAsync(string from, string to);
    Task SetAsync(ExchangeRate rate, TimeSpan ttl);
}
