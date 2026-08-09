using Application.Common.Interfaces;
using Application.Common.Models;
using Infrastructure.ExchangeRates.Clients;

namespace Infrastructure.ExchangeRates.Providers;

public class CoinGeckoExchangeRateProvider
    : IExchangeRateProvider
{
    private readonly CoinGeckoClient _client;

    public CoinGeckoExchangeRateProvider(CoinGeckoClient client)
    {
        _client = client;
    }

    public async Task<ExchangeRate> GetRateAsync(
        string from,
        string to,
        CancellationToken cancellationToken)
    {
        var rate = await _client.GetLatestRateAsync(
            from,
            to,
            cancellationToken);

        return new ExchangeRate(
            from,
            to,
            rate,
            DateTime.UtcNow);
    }
}
