using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace IntegrationTests.Infrastructure;

public sealed class HostedServicesTestFactory
    : WebApplicationFactory<Program>
{
    private readonly IntegrationFixture _fixture;

    public HostedServicesTestFactory(IntegrationFixture fixture)
    {
        _fixture = fixture;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("IntegrationTests");

        builder.ConfigureAppConfiguration(configuration =>
        {
            configuration.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:PostgreSQL"] = _fixture.PostgreSql.ConnectionString,
                ["ConnectionStrings:MongoDb"] = _fixture.Mongo.Container.GetConnectionString(),
                ["Kafka:BootstrapServers"] = _fixture.Kafka.BootstrapServers
            });
        });
    }
}
