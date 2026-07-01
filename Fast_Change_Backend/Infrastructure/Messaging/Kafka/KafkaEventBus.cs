using Application.Common.Interfaces;
using Confluent.Kafka;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Messaging.Kafka;

public sealed class KafkaEventBus : IEventBus
{
    private readonly IProducer<string, string> _producer;

    public KafkaEventBus(IProducer<string, string> producer)
    {
        _producer = producer;
    }

    public async Task PublishAsync<T>(
        Guid eventId,
        string topic,
        string key,
        T @event,
        CancellationToken cancellationToken = default)
    {
        var payload = JsonSerializer.Serialize(@event);

        await PublishAsync(eventId, topic, key, payload, cancellationToken);
    }

    public async Task PublishAsync(
        Guid eventId,
        string topic,
        string key,
        string payload,
        CancellationToken cancellationToken = default)
    {
        var message = new Message<string, string>
        {
            Key = key,
            Value = payload,
            Headers =
            [
                new Header("event-id", Encoding.UTF8.GetBytes(eventId.ToString()))
            ]
        };

        await _producer.ProduceAsync(topic, message, cancellationToken);
    }
}
