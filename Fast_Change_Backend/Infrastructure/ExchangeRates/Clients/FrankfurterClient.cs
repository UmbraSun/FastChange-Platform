using Infrastructure.ExchangeRates.Contracts;
using Resources;
using System.Net.Http.Json;

namespace Infrastructure.ExchangeRates.Clients;

public sealed class FrankfurterClient
{
    private readonly HttpClient _httpClient;

    public FrankfurterClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<FrankfurterResponse> GetLatestRateAsync(
        string from,
        string to,
        CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetFromJsonAsync<FrankfurterResponse>(
            $"latest?from={from}&to={to}",
            cancellationToken);

        if (response is null)
            throw new InvalidOperationException(Localization.ExchangeRateResponceIsEmpty);

        return response;
    }
}
