using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace Infrastructure.Messaging.Kafka;

public sealed class KafkaProducerFactory : IDisposable
{
    private readonly IProducer<string, string> _producer;

    public KafkaProducerFactory(IOptions<KafkaSettings> options)
    {
        var settings = options.Value;

        var config = new ProducerConfig
        {
            BootstrapServers = settings.BootstrapServers,
            ClientId = settings.ClientId,
            Acks = settings.Acks,
            EnableIdempotence = settings.EnableIdempotence,
            CompressionType = settings.CompressionType,
            MessageTimeoutMs = settings.MessageTimeoutMs
        };

        _producer = new ProducerBuilder<string, string>(config).Build();
        AdminClient = new AdminClientBuilder(config).Build();
    }

    public IProducer<string, string> Producer => _producer;
    
    public IAdminClient AdminClient { get; }

    public void Dispose()
    {
        _producer.Flush(TimeSpan.FromSeconds(10));
        _producer.Dispose();
    }
}
