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
        // Arrange
        var topic = $"test-topic-{Guid.NewGuid()}";
        var key = Guid.NewGuid().ToString();
        var value = "{\"message\":\"test\"}";

        var producerConfig = new ProducerConfig
        {
            BootstrapServers = _fixture.Kafka.BootstrapServers
        };

        using var producer = new ProducerBuilder<string, string>(producerConfig).Build();
        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = _fixture.Kafka.BootstrapServers,
            GroupId = $"test-group-{Guid.NewGuid()}",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using var consumer = new ConsumerBuilder<string, string>(consumerConfig).Build();
        consumer.Subscribe(topic);

        // Act
        await producer.ProduceAsync(topic,
            new Message<string, string>
            {
                Key = key,
                Value = value
            });

        var result = consumer.Consume(TimeSpan.FromSeconds(10));

        // Assert
        result.Should().NotBeNull();
        result!.Message.Key.Should().Be(key);
        result.Message.Value.Should().Be(value);
        consumer.Close();
    }
}
