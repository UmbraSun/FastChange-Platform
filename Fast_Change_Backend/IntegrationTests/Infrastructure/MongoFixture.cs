using MongoDB.Driver;
using Testcontainers.MongoDb;

namespace IntegrationTests.Infrastructure;

public sealed class MongoFixture : IAsyncLifetime
{
    public MongoDbContainer Container { get; } =
        new MongoDbBuilder("mongo:8")
            .WithUsername("root")
            .WithPassword("root")
            .Build();

    public IMongoDatabase Database =>
        new MongoClient(Container.GetConnectionString())
            .GetDatabase("FastChange");

    public async Task InitializeAsync()
    {
        await Container.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await Container.DisposeAsync();
    }
}
