using Contracts.Exceptions;
using Microsoft.Extensions.Logging;
using Resources;
using System.Net.Http.Json;

namespace Infrastructure.ExchangeRates.Clients;

public sealed class CoinGeckoClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CoinGeckoClient> _logger;

    public CoinGeckoClient(
        HttpClient httpClient,
        ILogger<CoinGeckoClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<decimal> GetLatestRateAsync(
        string from,
        string to,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Exchange rate request: {From} -> {To}", from, to);

            var fromIsCrypto = CurrencyHelper.IsCrypto(from);
            var toIsCrypto = CurrencyHelper.IsCrypto(to);

            if (!fromIsCrypto && !toIsCrypto)
                throw new ExternalServiceException(Localization.ExchangeRateNotFound);

            if (!fromIsCrypto && toIsCrypto)
            {
                var directRate = await GetRateAsync(to, from, cancellationToken);

                if (directRate == 0)
                {
                    _logger.LogError("CoinGecko returned zero rate: {From} -> {To}", to, from);
                    throw new ExternalServiceException(Localization.ExchangeRateNotFound);
                }

                var inverseRate = 1m / directRate;

                _logger.LogInformation("Inverse CoinGecko rate received: {From} -> {To}, Rate: {Rate}",
                    from,
                    to,
                    inverseRate);

                return inverseRate;
            }

            return await GetRateAsync(from, to, cancellationToken);
        }
        catch (TaskCanceledException)
        {
            _logger.LogError("CoinGecko request timeout");
            throw new ExternalServiceException(Localization.ExchangeRateProviderTimeout);
        }
        catch (Exception ex) when (ex is not ExternalServiceException)
        {
            _logger.LogError(ex, "Unexpected CoinGecko error");
            throw new ExternalServiceException(Localization.UnexpectedExchangeRateProviderError);
        }
    }

    private async Task<decimal> GetRateAsync(
        string from,
        string to,
        CancellationToken cancellationToken)
    {
        var fromId = CurrencyHelper.GetCoinGeckoId(from);
        var toCurrency = to.ToLowerInvariant();
        var response = await _httpClient.GetAsync($"api/v3/simple/price?ids={fromId}&vs_currencies={toCurrency}", cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("CoinGecko API error: {StatusCode}",response.StatusCode);
            throw new ExternalServiceException(Localization.ExchangeRateProviderUnavailable);
        }

        var rawData = await response.Content
            .ReadFromJsonAsync<Dictionary<string, Dictionary<string, decimal>>>(cancellationToken: cancellationToken);

        if (rawData is null)
        {
            _logger.LogError("CoinGecko returned empty response");
            throw new ExternalServiceException(Localization.EmptyResponseFromExchangeProvider);
        }

        if (!rawData.TryGetValue(fromId, out var rate))
        {
            _logger.LogError("CoinGecko currency not found: {Currency}", from);
            throw new ExternalServiceException(Localization.ExchangeRateNotFound);
        }

        if (!rate.TryGetValue(toCurrency, out var result))
        {
            _logger.LogError("CoinGecko rate not found: {From} -> {To}", from, to);
            throw new ExternalServiceException(Localization.ExchangeRateNotFound);
        }

        return result;
    }
}
