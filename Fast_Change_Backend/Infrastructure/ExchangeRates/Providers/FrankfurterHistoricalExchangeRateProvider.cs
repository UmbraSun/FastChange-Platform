using Application.Common.Models;
using Contracts.Exceptions;
using Infrastructure.ExchangeRates.Clients;
using Infrastructure.ExchangeRates.Exceptions;
using Resources;

namespace Infrastructure.ExchangeRates.Providers;

public sealed class FrankfurterHistoricalExchangeRateProvider
{
    private readonly FrankfurterClient _client;

    public FrankfurterHistoricalExchangeRateProvider(FrankfurterClient client)
    {
        _client = client;
    }

    public async Task<ExchangeRate> GetRateAsync(
        string fromCurrency,
        string toCurrency,
        DateOnly date,
        CancellationToken cancellationToken)
    {
        const int maxLookbackDays = 7;

        for (var daysBack = 0; daysBack <= maxLookbackDays; daysBack++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var requestedDate = date.AddDays(-daysBack);

            try
            {
                var rate = await _client.GetHistoricalRateAsync(fromCurrency, toCurrency, requestedDate, cancellationToken);

                return new ExchangeRate(fromCurrency, toCurrency, rate, requestedDate.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc));
            }
            catch (HistoricalRateNotAvailableException)
            {
                // Weekend / holiday.
                // Try the previous calendar day.
            }
        }

        throw new ExternalServiceException(Localization.ExchangeRateNotFound);
    }
}
