using Application.Common.Models;
using Contracts.Exceptions;
using Infrastructure.ExchangeRates.Contracts;
using Infrastructure.ExchangeRates.Exceptions;
using Microsoft.Extensions.Logging;
using Resources;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

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
            _logger.LogError("CoinGecko API error: {StatusCode}", response.StatusCode);
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

    public async Task<MarketData> GetMarketDataAsync(
        string currency,
        string targetCurrency,
        CancellationToken cancellationToken)
    {
        try
        {
            var currencyId = CurrencyHelper.GetCoinGeckoId(currency);
            var target = targetCurrency.ToLowerInvariant();

            _logger.LogInformation("Market data request: {Currency} -> {TargetCurrency}", currency, targetCurrency);

            var response = await _httpClient.GetAsync(
                $"api/v3/simple/price" +
                $"?ids={currencyId}" +
                $"&vs_currencies={target}" +
                "&include_24hr_change=true",
                cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("CoinGecko API error: {StatusCode}", response.StatusCode);
                throw new ExternalServiceException(Localization.ExchangeRateProviderUnavailable);
            }

            var rawData = await response.Content.ReadFromJsonAsync<
                Dictionary<string, Dictionary<string, JsonElement>>>(cancellationToken: cancellationToken);

            if (rawData is null)
            {
                _logger.LogError("CoinGecko returned empty response");
                throw new ExternalServiceException(Localization.EmptyResponseFromExchangeProvider);
            }

            if (!rawData.TryGetValue(currencyId, out var data))
            {
                _logger.LogError("CoinGecko currency not found: {Currency}", currency);
                throw new ExternalServiceException(Localization.ExchangeRateNotFound);
            }

            if (!data.TryGetValue(target, out var priceElement))
                throw new ExternalServiceException(Localization.ExchangeRateNotFound);

            if (!data.TryGetValue($"{target}_24h_change", out var changeElement))
            {
                _logger.LogError("CoinGecko 24h change not found: {Currency} -> {TargetCurrency}", currency, targetCurrency);
                throw new ExternalServiceException(Localization.ExchangeRateNotFound);
            }

            var price = priceElement.GetDecimal();
            var change24h = changeElement.GetDecimal();

            return new MarketData(currency, targetCurrency, price, change24h, DateTime.UtcNow);
        }
        catch (TaskCanceledException)
        {
            _logger.LogError("CoinGecko market data request timeout");
            throw new ExternalServiceException(Localization.ExchangeRateProviderTimeout);
        }
        catch (Exception ex) when (ex is not ExternalServiceException)
        {
            _logger.LogError(ex, "Unexpected CoinGecko market data error");
            throw new ExternalServiceException(Localization.UnexpectedExchangeRateProviderError);
        }
    }

    public async Task<decimal> GetHistoricalRateAsync(
        string from,
        string to,
        DateOnly date,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Historical CoinGecko rate request: {From} -> {To}, Date: {Date}", from, to, date);

            var fromIsCrypto = CurrencyHelper.IsCrypto(from);
            var toIsCrypto = CurrencyHelper.IsCrypto(to);

            if (!fromIsCrypto)
                throw new ExternalServiceException(Localization.ExchangeRateNotFound);

            var coinId = CurrencyHelper.GetCoinGeckoId(from);
            var response = await _httpClient.GetAsync($"api/v3/coins/{coinId}/history?date={date:dd-MM-yyyy}&localization=false", cancellationToken);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                _logger.LogInformation("CoinGecko historical rate not found: {From} -> {To}, Date: {Date}", from, to, date);
                throw new HistoricalRateNotAvailableException(from, to, date);
            }

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("CoinGecko historical API error: {StatusCode}", response.StatusCode);
                throw new ExternalServiceException(Localization.ExchangeRateProviderUnavailable);
            }

            var result = await response.Content.ReadFromJsonAsync<CoinGeckoHistoryResponse>(cancellationToken: cancellationToken);
            if (result?.MarketData?.CurrentPrice is null)
                throw new ExternalServiceException(Localization.EmptyResponseFromExchangeProvider);

            var targetCurrency = to.ToLowerInvariant();

            if (!result.MarketData.CurrentPrice.TryGetValue(
                    targetCurrency,
                    out var rate))
            {
                _logger.LogError("CoinGecko historical rate not found: {From} -> {To}, Date: {Date}", from, to, date);
                throw new ExternalServiceException(Localization.ExchangeRateNotFound);
            }

            if (rate <= 0)
                throw new ExternalServiceException(Localization.ExchangeRateNotFound);

            return rate;
        }
        catch (TaskCanceledException)
        {
            _logger.LogError("CoinGecko historical request timeout");
            throw new ExternalServiceException(Localization.ExchangeRateProviderTimeout);
        }
        catch (HistoricalRateNotAvailableException)
        {
            throw;
        }
        catch (Exception ex) when (ex is not ExternalServiceException)
        {
            _logger.LogError(ex, "Unexpected CoinGecko historical error");
            throw new ExternalServiceException(Localization.UnexpectedExchangeRateProviderError);
        }
    }
}
