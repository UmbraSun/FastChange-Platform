using Confluent.Kafka;

namespace IntegrationTests.Messaging;

public sealed class KafkaTestConsumer : IDisposable
{
    private readonly IConsumer<string, string> _consumer;

    public KafkaTestConsumer(string bootstrapServers)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = bootstrapServers,
            GroupId = Guid.NewGuid().ToString(),
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false
        };

        _consumer = new ConsumerBuilder<string, string>(config).Build();
    }

    public void Subscribe(string topic)
    {
        _consumer.Subscribe(topic);
    }

    public ConsumeResult<string, string>? Consume(
        TimeSpan timeout)
    {
        return _consumer.Consume(timeout);
    }

    public void Dispose()
    {
        _consumer.Close();
        _consumer.Dispose();
    }
}