using Microsoft.Extensions.DependencyInjection;
using Persistence;

namespace IntegrationTests.Extensions;

public static class ServiceProviderExtensions
{
    public static ApplicationDbContext GetDbContext(this IServiceProvider services)
        => services.GetRequiredService<ApplicationDbContext>();
}
