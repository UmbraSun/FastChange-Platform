using MongoDB.Driver;
using Testcontainers.MongoDb;

namespace IntegrationTests.Infrastructure;

public sealed class MongoFixture : IAsyncLifetime
{
    private IMongoClient? _client;

    public MongoDbContainer Container { get; } =
        new MongoDbBuilder("mongo:8")
            .WithUsername("root")
            .WithPassword("root")
            .Build();

    public string ConnectionString => Container.GetConnectionString();

    public IMongoDatabase Database => _client!.GetDatabase("FastChange");

    public async Task InitializeAsync()
    {
        await Container.StartAsync();
        
        _client = new MongoClient(Container.GetConnectionString());
    }

    public async Task DisposeAsync()
    {
        await Container.DisposeAsync();
    }
}
