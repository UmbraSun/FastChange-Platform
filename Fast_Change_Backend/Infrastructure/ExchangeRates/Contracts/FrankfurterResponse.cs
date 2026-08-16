using System.Text.Json.Serialization;

namespace Infrastructure.ExchangeRates.Contracts;

public sealed record FrankfurterResponse
{
    [JsonPropertyName("base")]
    public string Base { get; init; } = string.Empty;

    [JsonPropertyName("quote")]
    public string Quote { get; init; } = string.Empty;

    [JsonPropertyName("rate")]
    public decimal Rate { get; init; }

    [JsonPropertyName("date")]
    public DateOnly Date { get; init; }
}
