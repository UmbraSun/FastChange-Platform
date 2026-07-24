using Application.Common.Interfaces;
using IntegrationTests.Infrastructure.ExchangeRate;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace IntegrationTests.Infrastructure;

public sealed class ExchangeRateFailureFactory
    : IntegrationTestFactory
{
    public ExchangeRateFailureFactory(
        IntegrationFixture fixture)
        : base(fixture)
    {
    }

    protected override void ConfigureWebHost(
        IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<IExchangeRateProvider>();

            services.AddScoped<IExchangeRateProvider, FailedExchangeRateProvider>();
        });
    }
}
