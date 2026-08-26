using System.Text.Json.Serialization;

namespace Infrastructure.ExchangeRates.Contracts;

public sealed record CoinGeckoSimplePriceResponse
{
    [JsonPropertyName("usd")]
    public decimal Usd { get; init; }

    [JsonPropertyName("usd_24h_change")]
    public decimal? Usd24hChange { get; init; }
}
