using Application.Common.Exceptions;
using Infrastructure.ExchangeRates.Contracts;
using Microsoft.Extensions.Logging;
using Resources;
using System.Net.Http.Json;

namespace Infrastructure.ExchangeRates.Clients;

public sealed class FrankfurterClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<FrankfurterClient> _logger;

    public FrankfurterClient(
        HttpClient httpClient,
        ILogger<FrankfurterClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<FrankfurterResponse> GetLatestRateAsync(
        string from,
        string to,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation(
                "Exchange rate request: {From} -> {To}",
                from, to);

            var response = await _httpClient.GetAsync(
                $"latest?from={from}&to={to}",
                cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError(
                    "Frankfurter API error: {StatusCode}",
                    response.StatusCode);

                throw new ExternalServiceException(
                    Localization.ExchangeRateProviderUnavailable);
            }

            var result = await response.Content.ReadFromJsonAsync<FrankfurterResponse>(
                cancellationToken: cancellationToken);

            if (result is null)
            {
                _logger.LogError("Frankfurter returned empty response");

                throw new ExternalServiceException(
                    Localization.EmptyResponseFromExchangeProvider);
            }

            _logger.LogInformation(
                "Exchange rate received successfully: {From} -> {To}",
                from, to);

            return result;
        }
        catch (TaskCanceledException)
        {
            _logger.LogError("Frankfurter request timeout");

            throw new ExternalServiceException(
                Localization.ExchangeRateProviderTimeout);
        }
        catch (Exception ex) when (ex is not ExternalServiceException)
        {
            _logger.LogError(ex, "Unexpected Frankfurter error");

            throw new ExternalServiceException(
                Localization.UnexpectedExchangeRateProviderError);
        }
    }
}
