using Application.Common.Interfaces;
using Application.Common.Models;
using Infrastructure.ExchangeRates.Clients;

namespace Infrastructure.ExchangeRates.Providers;

public class CryptoRateProvider
    : IExchangeRateProvider
{
    private readonly CryptoRateClient _client;

    public CryptoRateProvider(CryptoRateClient client)
    {
        _client = client;
    }

    public async Task<ExchangeRate> GetRateAsync(
        string from,
        string to,
        CancellationToken cancellationToken)
    {
        var response = await _client.GetLatestRateAsync(
            from,
            to,
            cancellationToken);

        var rate = response.Rates[to];

        return new ExchangeRate(
            from,
            to,
            rate,
            DateTime.UtcNow);
    }
}
