using Application.Common.Interfaces;
using Application.Common.Models;
using Contracts.Exceptions;
using Infrastructure.ExchangeRates.Clients;
using Resources;

namespace Infrastructure.ExchangeRates.Providers;

public sealed class CoinGeckoMarketDataProvider
    : IMarketDataProvider, IBatchMarketDataProvider
{
    private static readonly TimeSpan CacheDuration = TimeSpan.FromSeconds(30);

    private readonly CoinGeckoClient _client;
    private readonly IMarketDataCache _cache;

    public CoinGeckoMarketDataProvider(
        CoinGeckoClient client,
        IMarketDataCache cache)
    {
        _client = client;
        _cache = cache;
    }

    public Task<MarketData> GetAsync(
        string currency,
        string targetCurrency,
        CancellationToken cancellationToken)
    {
        if (!CurrencyHelper.IsCrypto(currency))
            throw new ExternalServiceException(Localization.ExchangeRateNotFound);

        return _client.GetMarketDataAsync(currency, targetCurrency, cancellationToken);
    }

    public async Task<IReadOnlyList<MarketDataItem>> GetMarketDataAsync(
        IReadOnlyCollection<string> currencies,
        string quoteCurrency,
        CancellationToken cancellationToken)
    {
        var normalizedCurrencies = currencies
            .Select(x => x.ToUpperInvariant())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Order()
            .ToArray();

        var normalizedQuote = quoteCurrency.ToUpperInvariant();
        if (normalizedCurrencies.Length == 0)
            return [];

        var cacheKey = $"{normalizedQuote}:{string.Join(",", normalizedCurrencies)}";
        var cached = await _cache.GetAsync(cacheKey, cancellationToken);
        if (cached is not null)
            return cached;

        var data = await _client.GetMarketDataAsync(normalizedCurrencies, normalizedQuote, cancellationToken);
        var result = data.Select(x =>
        {
            var coinGeckoId = CurrencyHelper.GetCoinGeckoId(x.Key);

            return new MarketDataItem(
                coinGeckoId,
                normalizedQuote,
                x.Value.Usd,
                x.Value.Usd24hChange);
        }).ToList();

        await _cache.SetAsync(cacheKey, result, CacheDuration, cancellationToken);

        return result;
    }
}
