namespace Infrastructure.ExchangeRates;

public static class CurrencyHelper
{
    private static readonly Dictionary<string, string> CryptoMappings =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["BTC"] = "bitcoin",
            ["ETH"] = "ethereum",
            ["SOL"] = "solana",
            ["BNB"] = "binancecoin",
            ["XRP"] = "ripple",
            ["ADA"] = "cardano",
            ["DOT"] = "polkadot",
            ["LTC"] = "litecoin"
        };


    public static bool IsCrypto(string currency)
    {
        return CryptoMappings.ContainsKey(currency);
    }


    public static string GetCoinGeckoId(string ticker)
    {
        return CryptoMappings.TryGetValue(
            ticker,
            out var id)
            ? id
            : ticker.ToLowerInvariant();
    }
}
