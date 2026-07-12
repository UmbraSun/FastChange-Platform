using System.Diagnostics;

namespace Infrastructure.Observability;

/// <summary>
/// OpenTelemetry activity sources used by the application.
/// </summary>
public static class FastChangeTelemetry
{
    public const string SourceName = "FastChange";

    public static readonly ActivitySource ActivitySource =
        new(SourceName);
}
