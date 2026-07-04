using Application.Common.Interfaces;
using Confluent.Kafka;
using System.Text;

namespace Infrastructure.Messaging.Kafka;

public sealed class KafkaProducer : IKafkaProducer
{
    private readonly IProducer<string, string> _producer;

    public KafkaProducer(KafkaProducerFactory factory)
    {
        _producer = factory.Producer;
    }

    public async Task PublishAsync(
        string topic,
        string key,
        string value,
        IReadOnlyDictionary<string, string>? headers,
        CancellationToken cancellationToken)
    {
        var message = new Message<string, string>
        {
            Key = key,
            Value = value,
            Headers = BuildHeaders(headers)
        };

        await _producer.ProduceAsync(
            topic,
            message,
            cancellationToken);
    }

    private static Headers BuildHeaders(
        IReadOnlyDictionary<string, string>? headers)
    {
        var kafkaHeaders = new Headers();

        if (headers is null)
            return kafkaHeaders;

        foreach (var header in headers)
        {
            kafkaHeaders.Add(
                header.Key,
                Encoding.UTF8.GetBytes(header.Value));
        }

        return kafkaHeaders;
    }
}
