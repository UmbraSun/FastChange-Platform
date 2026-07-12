using Application.Common.Interfaces;
using Confluent.Kafka;
using Infrastructure.Observability;
using System.Diagnostics;
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
        using var activity =
            FastChangeTelemetry.ActivitySource.StartActivity(
                "Kafka Publish",
                ActivityKind.Producer);

        activity?.SetTag(
            "messaging.system",
            "kafka");

        activity?.SetTag(
            "messaging.destination.name",
            topic);

        activity?.SetTag(
            "messaging.message.key",
            key);

        var message = new Message<string, string>
        {
            Key = key,
            Value = value,
            Headers = BuildHeaders(headers)
        };

        try
        {
            var result = await _producer.ProduceAsync(topic, message, cancellationToken);

            activity?.SetTag(
                "messaging.kafka.partition", 
                result.Partition.Value);

            activity?.SetTag(
                "messaging.kafka.offset",
                result.Offset.Value);
        }
        catch (Exception ex)
        {
            activity?.SetStatus(ActivityStatusCode.Error);
            activity?.AddException(ex);
            throw;
        }
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
