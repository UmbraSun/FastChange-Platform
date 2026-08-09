namespace Application.Common.Settings;

public sealed class ExchangeRateSettings
{
    public const string SectionName = "ExchangeRate";

    public string CoinGeckoUrl { get; set; } = string.Empty;

    public string FrankfurterUrl { get; set; } = string.Empty;

    public int TimeoutSeconds { get; set; } = 10;
}
