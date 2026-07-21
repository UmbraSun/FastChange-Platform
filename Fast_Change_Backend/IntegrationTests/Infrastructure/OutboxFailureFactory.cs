using Application.Common.Interfaces;
using IntegrationTests.Infrastructure.Fakes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;


namespace IntegrationTests.Infrastructure;

public sealed class OutboxFailureFactory
    : IntegrationTestFactory
{
    public OutboxFailureFactory(IntegrationFixture fixture)
        : base(fixture)
    {
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<IKafkaProducer>();
            services.AddSingleton<IKafkaProducer, FailingKafkaProducer>();
        });
    }
}
