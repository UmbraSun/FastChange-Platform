using Confluent.Kafka;

namespace Infrastructure.Messaging.Kafka;

public sealed class KafkaSettings
{
    public const string SectionName = "Kafka";

    public string BootstrapServers { get; init; } = string.Empty;

    public string ClientId { get; init; } = "FastChange";

    public Acks Acks { get; init; } = Acks.All;

    public bool EnableIdempotence { get; init; } = true;

    public int MessageTimeoutMs { get; init; } = 30000;

    public CompressionType CompressionType { get; init; }
        = CompressionType.Lz4;
}
