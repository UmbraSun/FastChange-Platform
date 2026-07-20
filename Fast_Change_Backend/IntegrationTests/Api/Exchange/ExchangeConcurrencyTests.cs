using Application.Features.Exchange.ExchangeCurrency;
using Domain.Entities;
using FluentAssertions;
using IntegrationTests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Json;

namespace IntegrationTests.Api.Exchange;

public sealed class ExchangeConcurrencyTests
    : IntegrationTestBase
{
    public ExchangeConcurrencyTests(IntegrationFixture fixture)
        : base(fixture)
    {
    }

    [Fact]
    public async Task Exchange_Should_Handle_Concurrent_Requests_Safely()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var sourceWalletId = Guid.NewGuid();
        var destinationWalletId = Guid.NewGuid();

        await ExecuteScopeAsync(async db =>
        {
            var user = new User
            {
                Id = userId,
                Email = "concurrency@test.com",
                PasswordHash = "hash",
                IsVerified = true
            };

            var source = new Wallet
            {
                Id = sourceWalletId,
                UserId = userId,
                Currency = "USD"
            };
            source.Deposit(100);

            var destination = new Wallet
            {
                Id = destinationWalletId,
                UserId = userId,
                Currency = "BTC"
            };

            db.Users.Add(user);
            db.Wallets.AddRange(source, destination);
            await db.SaveChangesAsync();
        });

        // Act
        var command = new ExchangeCommand(sourceWalletId, destinationWalletId, 80);
        var firstRequest = Client.PostAsJsonAsync("/api/exchange", command);
        var secondRequest = Client.PostAsJsonAsync("/api/exchange", command);

        // Assert
        var responses = await Task.WhenAll(firstRequest, secondRequest);
        responses.Count(x => x.StatusCode == HttpStatusCode.OK).Should().Be(1);
        responses.Count(x => x.StatusCode == HttpStatusCode.Conflict).Should().Be(1);

        await ExecuteScopeAsync(async db =>
        {
            var sourceWallet = await db.Wallets.SingleAsync(x => x.Id == sourceWalletId);
            var destinationWallet = await db.Wallets.SingleAsync(x => x.Id == destinationWalletId);
            sourceWallet.Balance.Should().Be(20);
            destinationWallet.Balance.Should().BeGreaterThan(0);
            
            var transactions = await db.Transactions.CountAsync();
            transactions.Should().Be(2);

            var outbox = await db.OutboxMessages.CountAsync();
            outbox.Should().Be(1);
        });
    }
}
