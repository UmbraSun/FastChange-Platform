using Application.Common.Interfaces;

namespace IntegrationTests.Infrastructure.Fakes;

public sealed class FailingKafkaProducer : IKafkaProducer
{
    public Task PublishAsync(
        string topic,
        string key,
        string value,
        IReadOnlyDictionary<string, string>? headers,
        CancellationToken cancellationToken)
    {
        throw new Exception("Kafka unavailable");
    }
}
