using System.Text.Json.Serialization;

namespace Infrastructure.ExchangeRates.Contracts;

public sealed record CoinGeckoMarketDataResponse
{
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;

    [JsonPropertyName("symbol")]
    public string Symbol { get; init; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("current_price")]
    public decimal CurrentPrice { get; init; }

    [JsonPropertyName("price_change_percentage_24h")]
    public decimal? PriceChangePercentage24h { get; init; }
}
