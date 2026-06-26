namespace Application.Common.Settings;

public sealed class ExchangeRateSettings
{
    public const string SectionName = "ExchangeRate";

    public string BaseUrl { get; init; } = string.Empty;

    public int TimeoutSeconds { get; init; } = 10;
}
