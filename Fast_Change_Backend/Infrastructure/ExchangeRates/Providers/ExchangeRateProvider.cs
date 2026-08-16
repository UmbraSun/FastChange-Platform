using Application.Common.Interfaces;
using Application.Common.Models;
using Contracts.Exceptions;
using Resources;

namespace Infrastructure.ExchangeRates.Providers;

public sealed class ExchangeRateProvider 
    : IExchangeRateProvider
{
    private readonly FrankfurterExchangeRateProvider _fiatProvider;
    private readonly CoinGeckoExchangeRateProvider _cryptoProvider;

    public ExchangeRateProvider(
        FrankfurterExchangeRateProvider fiatProvider,
        CoinGeckoExchangeRateProvider cryptoProvider)
    {
        _fiatProvider = fiatProvider;
        _cryptoProvider = cryptoProvider;
    }

    public async Task<ExchangeRate> GetRateAsync(
        string from,
        string to,
        CancellationToken cancellationToken)
    {
        if (CurrencyHelper.IsCrypto(from))
            return await _cryptoProvider.GetRateAsync(from, to, cancellationToken);

        if (CurrencyHelper.IsCrypto(to))
        {
            var reverseRate = await _cryptoProvider.GetRateAsync(to, from, cancellationToken);
            if (reverseRate.Rate <= 0)
                throw new ExternalServiceException(Localization.ExchangeRateNotFound);
            
            return new ExchangeRate(from, to, 1m / reverseRate.Rate, reverseRate.RetrievedAtUtc);
        }

        return await _fiatProvider.GetRateAsync(from, to, cancellationToken);
    }
}
