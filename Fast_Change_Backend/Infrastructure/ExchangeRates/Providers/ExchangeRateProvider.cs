using Application.Common.Interfaces;
using Application.Common.Models;

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


    public Task<ExchangeRate> GetRateAsync(
        string from,
        string to,
        CancellationToken cancellationToken)
    {
        if (CurrencyHelper.IsCrypto(from))
            return _cryptoProvider.GetRateAsync(
                from,
                to,
                cancellationToken);

        return _fiatProvider.GetRateAsync(
            from,
            to,
            cancellationToken);
    }
}
