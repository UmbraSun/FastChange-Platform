namespace Application.Common.Settings;

public sealed class RedisSettings
{
    public const string SectionName = "Redis";

    public string ConnectionString { get; init; } = string.Empty;

    public int DefaultTtlSeconds { get; init; } = 60;
}
