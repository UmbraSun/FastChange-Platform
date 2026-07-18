namespace IntegrationTests.Infrastructure;

public sealed class IntegrationFixture : IAsyncLifetime
{
    public PostgreSqlFixture PostgreSql { get; } = new();

    public MongoFixture Mongo { get; } = new();

    public KafkaFixture Kafka { get; } = new();

    public DatabaseReset DatabaseReset { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        await PostgreSql.InitializeAsync();
        await Mongo.InitializeAsync();
        await Kafka.InitializeAsync();
        DatabaseReset = new DatabaseReset(PostgreSql.ConnectionString);
        await DatabaseReset.InitializeAsync();
    }

    public async Task DisposeAsync()
    {
        await Kafka.DisposeAsync();
        await Mongo.DisposeAsync();
        await PostgreSql.DisposeAsync();
    }
}
