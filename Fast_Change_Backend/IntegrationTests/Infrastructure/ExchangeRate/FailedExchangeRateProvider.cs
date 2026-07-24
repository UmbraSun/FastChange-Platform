using Application.Common.Interfaces;

namespace IntegrationTests.Infrastructure.ExchangeRate;

public sealed class FailedExchangeRateProvider
    : IExchangeRateProvider
{
    public Task<Application.Common.Models.ExchangeRate> GetRateAsync(
        string fromCurrency, 
        string toCurrency, 
        CancellationToken cancellationToken)
    {
        return Task.FromResult<Application.Common.Models.ExchangeRate>(null!);
    }
}
