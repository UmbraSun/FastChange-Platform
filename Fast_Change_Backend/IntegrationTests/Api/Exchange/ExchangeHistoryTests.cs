using Application.Features.Exchange.ExchangeCurrency;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Mongo.Documents;
using IntegrationTests.Extensions;
using IntegrationTests.Infrastructure;
using MongoDB.Driver;
using System.Net;
using System.Net.Http.Json;

namespace IntegrationTests.Api.Exchange;

public sealed class ExchangeHistoryTests
    : IntegrationTestBase
{
    public ExchangeHistoryTests(
        IntegrationFixture fixture)
        : base(fixture)
    {
    }

    [Fact]
    public async Task Exchange_Should_Create_WalletHistory_After_Exchange()
    {
        var userId = Guid.NewGuid();
        var fromWalletId = Guid.NewGuid();
        var toWalletId = Guid.NewGuid();

        await ExecuteScopeAsync(async db =>
        {
            var user = new User
            {
                Id = userId,
                Email = "history@test.com",
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
        var response = await Client.PostAsJsonAsync("/api/exchange", command);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await WaitExtensions.WaitUntilAsync(async () =>
        {
            var collection = Fixture.Mongo.Database
                .GetCollection<WalletHistoryDocument>("wallet-history");
            var count = await collection.CountDocumentsAsync(
                Builders<WalletHistoryDocument>.Filter.Empty);

            return count == 2;
        }, TimeSpan.FromSeconds(15));

        var history = await Fixture.Mongo.Database
            .GetCollection<WalletHistoryDocument>("wallet-history")
            .Find(Builders<WalletHistoryDocument>.Filter.Empty)
            .ToListAsync();

        history.Should().HaveCount(2);
        history.Should().Contain(x =>
            x.WalletId == fromWalletId &&
            x.SignedAmount == -100);
        history.Should().Contain(x =>
            x.WalletId == toWalletId &&
            x.SignedAmount == 1000);
    }
}