using Application.Common.Interfaces;
using IntegrationTests.Infrastructure;
using IntegrationTests.Infrastructure.Fakes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace IntegrationTests.Messaging.Outbox;

public sealed class OutboxRetryTestFactory
    : IntegrationTestFactory
{
    public OutboxRetryTestFactory(IntegrationFixture fixture)
        : base(fixture)
    {
    }

    protected override void ConfigureHostedServices(
       IServiceCollection services)
    {
    }

    protected override void ConfigureWebHost(
        IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<IKafkaProducer>();
            services.AddSingleton<IKafkaProducer, FailingKafkaProducer>();
        });
    }
}
