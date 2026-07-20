using Application.Common.Interfaces;
using Application.Common.Models;

namespace IntegrationTests.Infrastructure;

public sealed class TestExchangeRateProvider : IExchangeRateProvider
{
    public Task<ExchangeRate> GetRateAsync(
        string fromCurrency, 
        string toCurrency, 
        CancellationToken cancellationToken)
    {
        return Task.FromResult(
            new ExchangeRate(fromCurrency, toCurrency, 10m, DateTime.UtcNow));
    }
}
