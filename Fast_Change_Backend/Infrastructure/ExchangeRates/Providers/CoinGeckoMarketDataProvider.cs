using Application.Common.Interfaces;
using Application.Common.Models;
using Contracts.Exceptions;
using Infrastructure.ExchangeRates.Clients;
using Resources;

namespace Infrastructure.ExchangeRates.Providers;

public sealed class CoinGeckoMarketDataProvider
    : IMarketDataProvider
{
    private readonly CoinGeckoClient _client;

    public CoinGeckoMarketDataProvider(CoinGeckoClient client)
    {
        _client = client;
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
}
