using Testcontainers.PostgreSql;

namespace IntegrationTests.Infrastructure;

public sealed class PostgreSqlFixture : IAsyncLifetime
{
    public PostgreSqlContainer Container { get; }
        = new PostgreSqlBuilder("postgres:18")
            .WithDatabase("FastChange")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();

    public string ConnectionString
        => Container.GetConnectionString();

    public async Task InitializeAsync()
    {
        await Container.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await Container.DisposeAsync();
    }
}