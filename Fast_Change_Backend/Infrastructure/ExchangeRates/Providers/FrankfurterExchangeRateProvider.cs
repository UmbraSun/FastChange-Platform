using Application.Common.Interfaces;
using Application.Common.Models;
using Infrastructure.ExchangeRates.Clients;

namespace Infrastructure.ExchangeRates.Providers;

public class FrankfurterExchangeRateProvider 
    : IExchangeRateProvider
{
    private readonly FrankfurterClient _client;

    public FrankfurterExchangeRateProvider(
        FrankfurterClient client)
    {
        _client = client;
    }

    public async Task<ExchangeRate> GetRateAsync(
        string fromCurrency,
        string toCurrency,
        CancellationToken cancellationToken)
    {
        var response = await _client.GetLatestRateAsync(
            fromCurrency,
            toCurrency,
            cancellationToken);

        var rate = response.Rates[toCurrency];

        return new ExchangeRate(
            fromCurrency,
            toCurrency,
            rate,
            DateTime.UtcNow);
    }
}
