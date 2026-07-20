using Application.Features.Exchange.ExchangeCurrency;
using Contracts.Events;
using Domain.Entities;
using FluentAssertions;
using IntegrationTests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Json;

namespace IntegrationTests.Api.Exchange;

public sealed class ExchangeTests : IntegrationTestBase
{
    public ExchangeTests(IntegrationFixture fixture)
        : base(fixture)
    {
    }

    [Fact]
    public async Task Exchange_Should_Create_Transactions_And_Outbox_Message()
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
                Email = "exchange@test.com",
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

        await ExecuteScopeAsync(async db =>
        {
            var wallets = await db.Wallets
                .Where(x => x.Id == fromWalletId 
                         || x.Id == toWalletId)
                .ToListAsync();

            var transactions = await db.Transactions.ToListAsync();
            var outboxMessages = await db.OutboxMessages.ToListAsync();

            var sourceWallet = wallets.Single(x => x.Id == fromWalletId);
            var destinationWallet = wallets.Single(x => x.Id == toWalletId);

            sourceWallet.Balance.Should().Be(900);
            destinationWallet.Balance.Should().Be(1000);
            transactions.Should().HaveCount(2);
            transactions.Should().Contain(x =>
                x.OperationId != null &&
                x.SignedAmount == -100 &&
                x.ExchangeRate == 10);
            transactions.Should().Contain(x =>
                x.OperationId != null &&
                x.SignedAmount == 1000 &&
                x.ExchangeRate == 10);

            outboxMessages.Should().ContainSingle();
            outboxMessages[0].Type.Should().Contain(nameof(ExchangeCompletedEvent));
        });
    }
}
