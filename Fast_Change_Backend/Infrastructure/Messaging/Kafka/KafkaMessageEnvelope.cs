namespace Infrastructure.Messaging.Kafka;

public sealed class KafkaMessageEnvelope
{
    public string Topic { get; init; } = string.Empty;
    public string Key { get; init; } = string.Empty;
    public string Value { get; init; } = string.Empty;
    public Dictionary<string, string> Headers { get; init; } = new();
}
