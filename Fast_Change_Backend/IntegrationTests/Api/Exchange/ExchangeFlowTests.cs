using Application.Features.Auth.LoginUser;
using Application.Features.Exchange.ExchangeCurrency;
using FastChange.Application.Features.Users.RegisterUser;
using FluentAssertions;
using Infrastructure.Mongo.Documents;
using IntegrationTests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Persistence;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace IntegrationTests.Api.Exchange;

[Collection(nameof(IntegrationCollection))]
public sealed class ExchangeFlowTests
    : IAsyncLifetime
{
    private readonly IntegrationFixture _fixture;
    private readonly HostedServicesTestFactory _factory;
    private readonly HttpClient _client;

    public ExchangeFlowTests(IntegrationFixture fixture)
    {
        _fixture = fixture;
        _factory = new HostedServicesTestFactory(fixture);
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task Exchange_Should_Process_Full_Event_Flow()
    {
        // Arrange
        var email = "flow@test.com";
        var password = "Password123!";
        var registerCommand = new RegisterUserCommand(email, password);
        var registerResponse = await _client.PostAsJsonAsync("/api/auth/register", registerCommand);
        registerResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var loginCommand = new LoginUserCommand(email, password);
        var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", loginCommand);
        loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var loginResult = await loginResponse.Content.ReadFromJsonAsync<LoginUserResponse>();
        loginResult.Should().NotBeNull();
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",loginResult!.AccessToken);

        Guid fromWalletId;
        Guid toWalletId;
        
        await using (var scope = _factory.Services.CreateAsyncScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var user = await db.Users.Include(x => x.Wallets).SingleAsync(x => x.Email == email);
            fromWalletId = user.Wallets.Single(x => x.Currency == "USD").Id;
            toWalletId = user.Wallets.Single(x => x.Currency == "BTC").Id;
            user.Wallets.Single(x => x.Id == fromWalletId).Deposit(1000);
            await db.SaveChangesAsync();
        }
        
        var exchangeCommand = new ExchangeCommand(fromWalletId, toWalletId, 100);

        // Act
        var exchangeResponse = await _client.PostAsJsonAsync("/api/exchange", exchangeCommand);

        // Assert
        exchangeResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        await Task.Delay(3000);
        await using var verifyScope = _factory.Services.CreateAsyncScope();
        var context = verifyScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var outboxMessage = await context.OutboxMessages.SingleAsync();
        outboxMessage.ProcessedOnUtc.Should().NotBeNull();
        
        var mongo = _fixture.Mongo.Database;
        var history = await mongo.GetCollection<WalletHistoryDocument>("wallet-history")
            .Find(x => x.OperationType == "Exchange")
            .ToListAsync();
        history.Should().HaveCount(2);
        history.Should().Contain(x => x.SignedAmount == -100);
        history.Should().Contain(x => x.SignedAmount > 0);
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        _client.Dispose();
        await _factory.DisposeAsync();
    }
}
