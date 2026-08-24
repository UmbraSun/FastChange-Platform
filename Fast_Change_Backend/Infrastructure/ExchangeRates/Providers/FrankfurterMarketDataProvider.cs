using Application.Common.Interfaces;
using Application.Common.Models;
using Contracts.Exceptions;
using Infrastructure.ExchangeRates.Clients;
using Resources;

namespace Infrastructure.ExchangeRates.Providers;

public sealed class FrankfurterMarketDataProvider
    : IMarketDataProvider
{
    private readonly FrankfurterClient _client;

    public FrankfurterMarketDataProvider(FrankfurterClient client)
    {
        _client = client;
    }

    public async Task<MarketData> GetAsync(
        string currency,
        string targetCurrency,
        CancellationToken cancellationToken)
    {
        if (CurrencyHelper.IsCrypto(currency))
            throw new ExternalServiceException(Localization.ExchangeRateNotFound);

        if (currency.Equals(targetCurrency, StringComparison.OrdinalIgnoreCase))
            return new MarketData(currency, targetCurrency, 1m, 0m, DateTime.UtcNow);

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var previousDay = today.AddDays(-1);

        var currentRate = await _client.GetRateAsync(currency, targetCurrency, today, cancellationToken);
        var previousRate = await _client.GetRateAsync(currency, targetCurrency, previousDay, cancellationToken);

        if (previousRate <= 0)
            throw new ExternalServiceException(Localization.ExchangeRateNotFound);

        var change24h = (currentRate - previousRate) / previousRate * 100m;

        return new MarketData(currency, targetCurrency, currentRate, change24h, DateTime.UtcNow);
    }
}
