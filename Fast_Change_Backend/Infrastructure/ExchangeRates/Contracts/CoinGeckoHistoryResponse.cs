using System.Text.Json.Serialization;

namespace Infrastructure.ExchangeRates.Contracts;

public sealed record CoinGeckoHistoryResponse
{
    [JsonPropertyName("market_data")]
    public CoinGeckoMarketData? MarketData { get; init; }
}

public sealed record CoinGeckoMarketData
{
    [JsonPropertyName("current_price")]
    public Dictionary<string, decimal>? CurrentPrice { get; init; }
}
