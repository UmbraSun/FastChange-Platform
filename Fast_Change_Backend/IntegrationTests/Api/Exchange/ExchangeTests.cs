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

            fromWallet.Deposit(1000m);

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

        var command = new ExchangeCommand(fromWalletId, toWalletId, 100m);

        // Act
        var response = await Client.PostAsJsonAsync("/api/exchange", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<ExchangeResponse>();
        result.Should().NotBeNull();
        result!.ExchangeRate.Should().Be(10m);
        result.SentAmount.Should().Be(100m);
        result.ReceivedAmount.Should().Be(1000m);

        await ExecuteScopeAsync(async db =>
        {
            var wallets = await db.Wallets.Where(x =>
                x.Id == fromWalletId ||
                x.Id == toWalletId)
            .ToListAsync();

            var sourceWallet = wallets.Single(x => x.Id == fromWalletId);
            var destinationWallet = wallets.Single(x => x.Id == toWalletId);
            sourceWallet.Balance.Should().Be(900m);
            destinationWallet.Balance.Should().Be(1000m);

            var transactions = await db.Transactions.ToListAsync();
            transactions.Should().HaveCount(2);

            var operationIds = transactions.Select(x => x.OperationId).Distinct().ToList();
            operationIds.Should().ContainSingle();
            operationIds[0].Should().NotBeNull();

            transactions.Should().Contain(x =>
                x.SignedAmount == -100m &&
                x.ExchangeRate == 10m);
            transactions.Should().Contain(x =>
                x.SignedAmount == 1000m &&
                x.ExchangeRate == 10m);

            var outboxMessages = await db.OutboxMessages.ToListAsync();
            outboxMessages.Should().ContainSingle();
            
            var outbox = outboxMessages[0];
            outbox.Type.Should().Contain(nameof(ExchangeCompletedEvent));
            outbox.ProcessedOnUtc.Should().BeNull();
        });
    }

    [Fact]
    public async Task Exchange_Should_Return_Conflict_When_Wallet_Not_Found()
    {
        // Arrange
        var command = new ExchangeCommand(Guid.NewGuid(), Guid.NewGuid(), 100m);

        // Act
        var response = await Client.PostAsJsonAsync("/api/exchange", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Exchange_Should_Return_Conflict_When_Source_And_Destination_Wallets_Are_Same()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var walletId = Guid.NewGuid();

        await ExecuteScopeAsync(async db =>
        {
            var user = new User
            {
                Id = userId,
                Email = "same-wallet@test.com",
                PasswordHash = "hash",
                IsVerified = true
            };

            var wallet = new Wallet
            {
                Id = walletId,
                UserId = userId,
                Currency = "USD"
            };

            wallet.Deposit(1000m);

            db.Users.Add(user);
            db.Wallets.Add(wallet);
            await db.SaveChangesAsync();
        });

        var command = new ExchangeCommand(walletId, walletId, 100m);

        // Act
        var response = await Client.PostAsJsonAsync("/api/exchange", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);

        await ExecuteScopeAsync(async db =>
        {
            var wallet = await db.Wallets.SingleAsync(x => x.Id == walletId);
            wallet.Balance.Should().Be(1000m);

            var transactions = await db.Transactions.ToListAsync();
            transactions.Should().BeEmpty();

            var outboxMessages = await db.OutboxMessages.ToListAsync();
            outboxMessages.Should().BeEmpty();
        });
    }

    [Fact]
    public async Task Exchange_Should_Return_InternalServerError_When_Insufficient_Funds()
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
                Email = "insufficient@test.com",
                PasswordHash = "hash",
                IsVerified = true
            };

            var fromWallet = new Wallet
            {
                Id = fromWalletId,
                UserId = userId,
                Currency = "USD"
            };

            fromWallet.Deposit(50m);

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

        var command = new ExchangeCommand(fromWalletId, toWalletId, 100m);

        // Act
        var response = await Client.PostAsJsonAsync("/api/exchange", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

        await ExecuteScopeAsync(async db =>
        {
            var wallets = await db.Wallets.Where(x =>
                x.Id == fromWalletId ||
                x.Id == toWalletId)
            .ToListAsync();
            wallets.Single(x => x.Id == fromWalletId).Balance.Should().Be(50m);
            wallets.Single(x => x.Id == toWalletId).Balance.Should().Be(0m);

            var transactions = await db.Transactions.ToListAsync();
            transactions.Should().BeEmpty();

            var outbox = await db.OutboxMessages.ToListAsync();
            outbox.Should().BeEmpty();
        });
    }
}
