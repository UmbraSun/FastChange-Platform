using FluentAssertions;
using IntegrationTests.Extensions;
using IntegrationTests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

namespace IntegrationTests.Messaging.Outbox;

[Collection(nameof(IntegrationCollection))]
public sealed class OutboxIntegrationTests : IAsyncLifetime
{
    private readonly IntegrationFixture _fixture;
    private readonly HostedServicesTestFactory _factory;
    private readonly HttpClient _client;

    public OutboxIntegrationTests(IntegrationFixture fixture)
    {
        _fixture = fixture;
        _factory = new HostedServicesTestFactory(fixture);
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task OutboxDispatcher_Should_Mark_Message_As_Processed()
    {
        // Arrange
        await IntegrationTestsHelper.WaitAsync(async () =>
        {
            await using var scope = _factory.Services.CreateAsyncScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            return await db.OutboxMessages.AnyAsync(x => x.ProcessedOnUtc != null);
        });

        // Act
        await using var scope = _factory.Services.CreateAsyncScope();

        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var message = await db.OutboxMessages.SingleAsync();
        message.ProcessedOnUtc.Should().NotBeNull();
    }

    public async Task InitializeAsync()
    {
        await _fixture.DatabaseReset.ResetAsync();
    }

    public async Task DisposeAsync()
    {
        _client.Dispose();
        _factory.Dispose();
    }
}
