using Application.Features.Exchange.ExchangeCurrency;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Mongo.Documents;
using IntegrationTests.Infrastructure;
using MongoDB.Driver;
using System.Net;
using System.Net.Http.Json;

namespace IntegrationTests.Api.Exchange;

public sealed class ExchangePipelineTests
    : IntegrationTestBase
{
    public ExchangePipelineTests(IntegrationFixture fixture)
        : base(fixture)
    {
    }

    [Fact]
    public async Task Exchange_Should_Process_Full_Messaging_Pipeline()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var fromWalletId = Guid.NewGuid();
        var toWalletId = Guid.NewGuid();

        await ExecuteScopeAsync(async db =>
        {
            var user = new User
            {
                Id = userId,
                Email = "pipeline@test.com",
                PasswordHash = "hash",
                IsVerified = true
            };

            var fromWallet = new Wallet
            {
                Id = fromWalletId,
                UserId = userId,
                Currency = "USD"
            };

            fromWallet.Deposit(1000);

            var toWallet = new Wallet
            {
                Id = toWalletId,
                UserId = userId,
                Currency = "BTC"
            };

            db.Users.Add(user);
            db.Wallets.AddRange(fromWallet, toWallet);

            await db.SaveChangesAsync();
        });

        var command = new ExchangeCommand(fromWalletId, toWalletId, 100);

        // Act
        var response = await Client.PostAsJsonAsync("/api/exchange", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var mongo = Fixture.Mongo.Database.GetCollection<WalletHistoryDocument>("wallet-history");
        var history = new List<WalletHistoryDocument>();
        var timeout = DateTime.UtcNow.AddSeconds(15);

        while (DateTime.UtcNow < timeout)
        {
            history = await mongo.Find(x => 
                x.WalletId == fromWalletId ||
                x.WalletId == toWalletId)
                .ToListAsync();

            if (history.Count == 2) break;

            await Task.Delay(250);
        }

        history.Should().HaveCount(2);
        history.Should().Contain(x =>
            x.WalletId == fromWalletId &&
            x.SignedAmount == -100);
        history.Should().Contain(x =>
            x.WalletId == toWalletId &&
            x.SignedAmount == 1000);

        await ExecuteScopeAsync(async db => db.OutboxMessages.Should().OnlyContain(x => x.ProcessedOnUtc != null));
    }
}
