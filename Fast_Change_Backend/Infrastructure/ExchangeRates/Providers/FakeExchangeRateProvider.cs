using Application.Common.Interfaces;
using Application.Common.Models;
using Resources;

namespace Infrastructure.ExchangeRates.Providers;

public sealed class FakeExchangeRateProvider
    : IExchangeRateProvider
{
    private static readonly Dictionary<(string From, string To), decimal> Rates =
        new()
        {
            { ("USD", "BTC"), 0.0000084m },
            { ("BTC", "USD"), 119000m },

            { ("USD", "EUR"), 0.85m },
            { ("EUR", "USD"), 1.17m },

            { ("BTC", "EUR"), 101000m },
            { ("EUR", "BTC"), 0.0000099m }
        };

    public Task<ExchangeRate> GetRateAsync(
        string fromCurrency,
        string toCurrency,
        CancellationToken cancellationToken)
    {
        if (fromCurrency == toCurrency)
            return Task.FromResult(new ExchangeRate(
                string.Empty,
                string.Empty,
                decimal.Zero,
                DateTime.MinValue));

        if (!Rates.TryGetValue(
                (fromCurrency.ToUpperInvariant(),
                 toCurrency.ToUpperInvariant()),
                out var rate))
            throw new InvalidOperationException(
                string.Format(Localization.RateNotFound, fromCurrency, toCurrency));

        return Task.FromResult(
            new ExchangeRate(
                fromCurrency,
                toCurrency,
                rate,
                DateTime.UtcNow));
    }
}
