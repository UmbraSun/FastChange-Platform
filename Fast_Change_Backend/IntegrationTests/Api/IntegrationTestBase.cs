using IntegrationTests.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

namespace IntegrationTests.Api;

[Collection(nameof(IntegrationCollection))]
public abstract class IntegrationTestBase : IAsyncLifetime
{
    protected readonly HttpClient Client;
    protected readonly IntegrationFixture Fixture;
    protected readonly IntegrationTestFactory Factory;

    protected IntegrationTestBase(IntegrationFixture fixture)
    {
        Fixture = fixture;

        Factory = new IntegrationTestFactory(fixture);
        Client = Factory.CreateClient();
    }

    protected async Task ExecuteScopeAsync(
        Func<ApplicationDbContext, Task> action)
    {
        using var scope = Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await action(db);
    }

    protected async Task<TResult> ExecuteScopeAsync<TResult>(
        Func<ApplicationDbContext, Task<TResult>> action)
    {
        using var scope = Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        return await action(db);
    }

    public virtual async Task InitializeAsync()
    {
        await Fixture.DatabaseReset.ResetAsync();
    }

    public virtual Task DisposeAsync()
    {
        Client.Dispose();
        return Task.CompletedTask;
    }
}
