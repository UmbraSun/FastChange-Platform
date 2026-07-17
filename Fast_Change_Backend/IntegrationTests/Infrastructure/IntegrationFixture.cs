namespace IntegrationTests.Infrastructure;

public sealed class IntegrationFixture : IAsyncLifetime
{
    public PostgreSqlFixture PostgreSql { get; } = new();

    public MongoFixture Mongo { get; } = new();

    public KafkaFixture Kafka { get; } = new();

    public async Task InitializeAsync()
    {
        await PostgreSql.InitializeAsync();
        await Mongo.InitializeAsync();
        await Kafka.InitializeAsync();
    }

    public async Task DisposeAsync()
    {
        await Kafka.DisposeAsync();
        await Mongo.DisposeAsync();
        await PostgreSql.DisposeAsync();
    }
}
