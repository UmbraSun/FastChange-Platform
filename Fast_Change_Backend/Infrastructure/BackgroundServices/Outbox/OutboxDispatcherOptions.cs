namespace Infrastructure.BackgroundServices.Outbox;

public sealed class OutboxDispatcherOptions
{
    public const string SectionName = "Outbox";

    public int BatchSize { get; init; } = 100;

    public int PollingIntervalMs { get; init; } = 1000;
}
