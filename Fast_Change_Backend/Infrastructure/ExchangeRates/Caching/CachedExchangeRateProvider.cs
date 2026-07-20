using Application.Common.Interfaces;
using Application.Common.Models;

namespace Infrastructure.ExchangeRates.Caching;

public sealed class CachedExchangeRateProvider : IExchangeRateProvider
{
    private readonly IExchangeRateProvider _inner;
    private readonly IExchangeRateCache _cache;

    public CachedExchangeRateProvider(
        IExchangeRateProvider inner,
        IExchangeRateCache cache)
    {
        _inner = inner;
        _cache = cache;
    }

    public async Task<ExchangeRate> GetRateAsync(
        string fromCurrency,
        string toCurrency,
        CancellationToken cancellationToken)
    {
        var cached = await _cache.GetAsync(fromCurrency, toCurrency);
        if (cached is not null) return cached;
        
        var rate = await _inner.GetRateAsync(fromCurrency, toCurrency, cancellationToken);
        await _cache.SetAsync(rate, TimeSpan.FromMinutes(1));
        return rate;
    }
}
