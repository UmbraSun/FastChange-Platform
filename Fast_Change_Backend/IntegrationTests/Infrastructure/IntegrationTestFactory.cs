using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace IntegrationTests.Infrastructure;

public sealed class IntegrationTestFactory
    : WebApplicationFactory<Program>
{
    private readonly IntegrationFixture _fixture;

    public IntegrationTestFactory(IntegrationFixture fixture)
    {
        _fixture = fixture;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("IntegrationTests");

        builder.ConfigureAppConfiguration(configuration =>
        {
            configuration.AddInMemoryCollection(
                new Dictionary<string, string?>
                {
                    ["ConnectionStrings:PostgreSQL"] = _fixture.PostgreSql.ConnectionString,
                    ["ConnectionStrings:MongoDb"] = _fixture.Mongo.Container.GetConnectionString(),
                    ["Kafka:BootstrapServers"] = _fixture.Kafka.BootstrapServers
                });
        });

        builder.ConfigureServices(services => RemoveHostedServices(services));
    }

    private static void RemoveHostedServices(IServiceCollection services)
    {
        services.RemoveAll<IHostedService>();
    }
}
