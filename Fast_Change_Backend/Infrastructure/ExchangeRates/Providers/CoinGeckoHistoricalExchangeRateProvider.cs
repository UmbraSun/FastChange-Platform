using Application.Common.Interfaces;
using Application.Common.Models;
using Infrastructure.ExchangeRates.Clients;

namespace Infrastructure.ExchangeRates.Providers;

public sealed class CoinGeckoHistoricalExchangeRateProvider
    : IHistoricalExchangeRateProvider
{
    private readonly CoinGeckoClient _client;

    public CoinGeckoHistoricalExchangeRateProvider(CoinGeckoClient client)
    {
        _client = client;
    }

    public async Task<ExchangeRate> GetRateAsync(
        string fromCurrency,
        string toCurrency,
        DateOnly date,
        CancellationToken cancellationToken)
    {
        var rate = await _client.GetHistoricalRateAsync(fromCurrency, toCurrency, date, cancellationToken);
        
        return new ExchangeRate(fromCurrency, toCurrency, rate, date.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc));
    }
}
