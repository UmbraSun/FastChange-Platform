using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace Infrastructure.Messaging.Kafka.Consumers;

public sealed class KafkaConsumerFactory : IDisposable
{
    private readonly IConsumer<string, string> _consumer;

    public KafkaConsumerFactory(IOptions<KafkaSettings> options)
    {
        var settings = options.Value;

        var config = new ConsumerConfig
        {
            BootstrapServers = settings.BootstrapServers,
            ClientId = settings.ClientId,
            GroupId = settings.GroupId,

            AutoOffsetReset = settings.AutoOffsetReset,

            EnableAutoCommit = false,
            EnableAutoOffsetStore = false
        };

        _consumer = new ConsumerBuilder<string, string>(config).Build();
    }

    public IConsumer<string, string> Consumer => _consumer;

    public void Dispose()
    {
        _consumer.Close();
        _consumer.Dispose();
    }
}
