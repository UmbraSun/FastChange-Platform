using Application.Common.Interfaces;
using BuildingBlocks.Messaging;
using Contracts.Events;
using Infrastructure.Messaging.Handlers;
using IntegrationTests.Authentication;
using IntegrationTests.Infrastructure.Fakes;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace IntegrationTests.Infrastructure;

public class IntegrationTestFactory 
    : WebApplicationFactory<Program>
{
    private readonly IntegrationFixture _fixture;

    public IntegrationTestFactory(IntegrationFixture fixture)
    {
        _fixture = fixture;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment(nameof(IntegrationTests));

        builder.ConfigureAppConfiguration(configuration =>
        {
            configuration.AddInMemoryCollection(
                new Dictionary<string, string?>
                {
                    ["ConnectionStrings:DefaultConnection"] = _fixture.PostgreSql.ConnectionString,
                    ["Mongo:ConnectionStrings"] = _fixture.Mongo.Container.GetConnectionString(),
                    ["Mongo:DatabaseName"] = "FastChange",
                    ["Kafka:BootstrapServers"] = _fixture.Kafka.BootstrapServers
                });
        });

        builder.ConfigureServices(services =>
        {
            RemoveHostedServices(services);
            
            services.RemoveAll<IIntegrationEventHandler<ExchangeCompletedEvent>>();
            services.AddScoped<IIntegrationEventHandler<ExchangeCompletedEvent>, ExchangeCompletedHandler>();
            services.AddScoped<IIntegrationEventHandler<ExchangeCompletedEvent>, FakeExchangeCompletedHandler>();
            
            services.RemoveAll<IKafkaProducer>();
            services.AddSingleton<IKafkaProducer, FailingKafkaProducer>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = TestAuthenticationHandler.Scheme;
                options.DefaultChallengeScheme = TestAuthenticationHandler.Scheme;
            })
            .AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>(TestAuthenticationHandler.Scheme, _ => { });
        });

        builder.ConfigureTestServices(services =>
        {
            services.PostConfigure<AuthenticationOptions>(options =>
            {
                options.DefaultAuthenticateScheme = TestAuthenticationHandler.Scheme;
                options.DefaultChallengeScheme = TestAuthenticationHandler.Scheme;
            });

            services.RemoveAll<IExchangeRateProvider>();
            services.AddScoped<IExchangeRateProvider, TestExchangeRateProvider>();
        });
    }

    private static void RemoveHostedServices(IServiceCollection services)
    {
        services.RemoveAll<IHostedService>();
    }
}
