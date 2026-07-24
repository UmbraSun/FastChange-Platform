using Application.Common.Interfaces;
using Infrastructure.Messaging.Kafka;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace IntegrationTests.Infrastructure;

public sealed class MessagingTestFactory
    : IntegrationTestFactory
{
    public MessagingTestFactory(IntegrationFixture fixture)
        : base(fixture)
    {
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<IKafkaProducer>();
            services.AddSingleton<IKafkaProducer, KafkaProducer>();
        });
    }

    protected override void ConfigureHostedServices(IServiceCollection services)
    {
    }
}
