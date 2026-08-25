using Application.Common.Interfaces;
using Application.Common.Models;
using Contracts.Exceptions;
using Resources;

namespace Infrastructure.ExchangeRates.Providers;

public sealed class HistoricalExchangeRateProvider
    : IHistoricalExchangeRateProvider
{
    private readonly FrankfurterHistoricalExchangeRateProvider _fiatProvider;
    private readonly CoinGeckoHistoricalExchangeRateProvider _cryptoProvider;

    public HistoricalExchangeRateProvider(
        FrankfurterHistoricalExchangeRateProvider fiatProvider,
        CoinGeckoHistoricalExchangeRateProvider cryptoProvider)
    {
        _fiatProvider = fiatProvider;
        _cryptoProvider = cryptoProvider;
    }

    public Task<ExchangeRate> GetRateAsync(
        string fromCurrency,
        string toCurrency,
        DateOnly date,
        CancellationToken cancellationToken)
    {
        if (CurrencyHelper.IsCrypto(fromCurrency))
            return _cryptoProvider.GetRateAsync(fromCurrency, toCurrency, date, cancellationToken);

        if (CurrencyHelper.IsCrypto(toCurrency))
            return GetFiatToCryptoRateAsync(fromCurrency, toCurrency, date, cancellationToken);

        return _fiatProvider.GetRateAsync(fromCurrency, toCurrency, date, cancellationToken);
    }

    private async Task<ExchangeRate> GetFiatToCryptoRateAsync(
        string fromCurrency,
        string toCurrency,
        DateOnly date,
        CancellationToken cancellationToken)
    {
        var reverseRate = await _cryptoProvider.GetRateAsync(toCurrency, fromCurrency, date, cancellationToken);

        if (reverseRate.Rate <= 0)
            throw new ExternalServiceException(Localization.ExchangeRateNotFound);

        return new ExchangeRate(fromCurrency, toCurrency, 1m / reverseRate.Rate, reverseRate.RetrievedAtUtc);
    }
}
