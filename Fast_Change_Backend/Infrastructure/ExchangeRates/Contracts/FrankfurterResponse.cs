using System.Text.Json.Serialization;

namespace Infrastructure.ExchangeRates.Contracts;

public sealed record FrankfurterResponse
{
    [JsonPropertyName("base")]
    public string Base { get; init; } = string.Empty;

    [JsonPropertyName("date")]
    public DateOnly Date { get; init; }

    [JsonPropertyName("rates")]
    public Dictionary<string, decimal> Rates { get; init; } = [];
}
