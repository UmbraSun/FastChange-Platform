using Testcontainers.Kafka;

namespace IntegrationTests.Infrastructure;

public sealed class KafkaFixture : IAsyncLifetime
{
    public KafkaContainer Container { get; } 
        = new KafkaBuilder("confluentinc/cp-kafka:7.8.0").Build();

    public string BootstrapServers =>
        Container.GetBootstrapAddress();

    public async Task InitializeAsync()
    {
        await Container.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await Container.DisposeAsync();
    }
}
