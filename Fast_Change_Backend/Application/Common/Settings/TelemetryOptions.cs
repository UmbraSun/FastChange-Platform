namespace Application.Common.Settings;

/// <summary>
/// OpenTelemetry settings.
/// </summary>
public sealed class TelemetryOptions
{
    public const string SectionName = "Telemetry";

    public string ServiceName { get; init; }
        = "FastChange.Api";

    public string ServiceVersion { get; init; }
        = "1.0.0";
}
