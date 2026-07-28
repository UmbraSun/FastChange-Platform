using Application.Common.Interfaces;

namespace IntegrationTests.Infrastructure.Fakes;

public sealed class FailingKafkaProducer : IKafkaProducer
{
    public static bool ShouldFail { get; set; }

    public Task PublishAsync(
        string topic,
        string key,
        string value,
        IReadOnlyDictionary<string, string>? headers,
        CancellationToken cancellationToken)
    {
        if (ShouldFail)
            throw new Exception("Kafka unavailable");

        return Task.CompletedTask;
    }
}
