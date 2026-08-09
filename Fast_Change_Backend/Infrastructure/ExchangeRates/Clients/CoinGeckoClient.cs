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
            var fromId = CurrencyHelper.GetCoinGeckoId(from);
            var toId = CurrencyHelper.GetCoinGeckoId(to);

            _logger.LogInformation(
                "Exchange rate request: {From} -> {To}",
                from, to);

            var response = await _httpClient.GetAsync(
                $"api/v3/simple/price?ids={fromId}&vs_currencies={toId}",
                cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError(
                    "CoinGecko API error: {StatusCode}",
                    response.StatusCode);

                throw new ExternalServiceException(
                    Localization.ExchangeRateProviderUnavailable);
            }

            var rawData = await response.Content.ReadFromJsonAsync<Dictionary<string, Dictionary<string, decimal>>>(
                cancellationToken: cancellationToken);

            if (rawData is null)
            {
                _logger.LogError("CoinGecko returned empty response");
                throw new ExternalServiceException(Localization.EmptyResponseFromExchangeProvider);
            }

            rawData.TryGetValue(fromId, out var rate);

            if (rate is null)
            {
                _logger.LogError("CoinGecko returned empty response");

                throw new ExternalServiceException(
                    Localization.EmptyResponseFromExchangeProvider);
            }

            if (!rate.TryGetValue(
                toId,
                out var result))
            {
                _logger.LogError(
                    "CoinGecko rate not found: {From} -> {To}",
                    from,
                    to);

                throw new ExternalServiceException(
                    Localization.ExchangeRateNotFound);
            }

            _logger.LogInformation(
                "Exchange rate received successfully: {From} -> {To}",
                from, to);

            return result;
        }
        catch (TaskCanceledException)
        {
            _logger.LogError("CoinGecko request timeout");

            throw new ExternalServiceException(
                Localization.ExchangeRateProviderTimeout);
        }
        catch (Exception ex) when (ex is not ExternalServiceException)
        {
            _logger.LogError(ex, "Unexpected CoinGecko error");

            throw new ExternalServiceException(
                Localization.UnexpectedExchangeRateProviderError);
        }
    }
}
