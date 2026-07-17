using Confluent.Kafka;
using FluentAssertions;
using IntegrationTests.Infrastructure;

namespace IntegrationTests.Kafka;

[Collection(nameof(KafkaCollection))]
public sealed class KafkaTests
{
    private readonly IntegrationFixture _fixture;

    public KafkaTests(IntegrationFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Kafka_Should_Produce_And_Consume_Message()
    {
        const string topic = "integration-tests";

        var producer = new ProducerBuilder<string, string>(
            new ProducerConfig
            {
                BootstrapServers = _fixture.Kafka.BootstrapServers
            })
            .Build();

        await producer.ProduceAsync(topic,
            new Message<string, string>
            {
                Key = "1",
                Value = "Hello Kafka"
            });

        producer.Flush(TimeSpan.FromSeconds(5));

        var consumer = new ConsumerBuilder<string, string>(
            new ConsumerConfig
            {
                BootstrapServers = _fixture.Kafka.BootstrapServers,
                GroupId = Guid.NewGuid().ToString(),
                AutoOffsetReset = AutoOffsetReset.Earliest
            })
            .Build();

        consumer.Subscribe(topic);

        var result = consumer.Consume(TimeSpan.FromSeconds(10));

        result.Should().NotBeNull();
        result!.Message.Value.Should().Be("Hello Kafka");

        consumer.Close();
    }
}
