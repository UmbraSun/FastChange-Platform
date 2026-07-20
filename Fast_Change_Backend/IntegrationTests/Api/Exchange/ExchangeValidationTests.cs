using Application.Features.Exchange.ExchangeCurrency;
using Domain.Entities;
using FluentAssertions;
using IntegrationTests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Json;

namespace IntegrationTests.Api.Exchange;

public sealed class ExchangeValidationTests 
    : IntegrationTestBase
{
    public ExchangeValidationTests(IntegrationFixture fixture)
        : base(fixture)
    {
    }

    [Fact]
    public async Task Exchange_Should_Return_Error_When_Balance_Is_Insufficient()
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
            fromWallet.Deposit(50);

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
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        
        await ExecuteScopeAsync(async db =>
        {
            var wallets = await db.Wallets.Where(x =>
                x.Id == fromWalletId ||
                x.Id == toWalletId).ToListAsync();

            wallets.Single(x => x.Id == fromWalletId).Balance.Should().Be(50);
            wallets.Single(x => x.Id == toWalletId).Balance.Should().Be(0);
            (await db.Transactions.CountAsync()).Should().Be(0);
            (await db.OutboxMessages.CountAsync()).Should().Be(0);
        });
    }

    [Fact]
    public async Task Exchange_Should_Return_Error_When_Wallets_Are_Equal()
    {
        // Arrange
        var walletId = Guid.NewGuid();

        await ExecuteScopeAsync(async db =>
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = "same-wallet@test.com",
                PasswordHash = "hash",
                IsVerified = true
            };

            var wallet = new Wallet
            {
                Id = walletId,
                UserId = user.Id,
                Currency = "USD"
            };

            wallet.Deposit(100);
            db.Users.Add(user);
            db.Wallets.Add(wallet);
            await db.SaveChangesAsync();
        });

        var command = new ExchangeCommand(walletId, walletId, 10);

        // Act
        var response = await Client.PostAsJsonAsync("/api/exchange", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);

        await ExecuteScopeAsync(async db =>
        {
            (await db.Transactions.CountAsync()).Should().Be(0);
            (await db.OutboxMessages.CountAsync()).Should().Be(0);
        });
    }

    [Fact]
    public async Task Exchange_Should_Return_Conflict_When_Wallet_Does_Not_Exist()
    {
        // Arrange
        var command = new ExchangeCommand(Guid.NewGuid(), Guid.NewGuid(), 100);

        // Act
        var response = await Client.PostAsJsonAsync(
            "/api/exchange",
            command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);

        await ExecuteScopeAsync(async db =>
        {
            (await db.Transactions.CountAsync()).Should().Be(0);
            (await db.OutboxMessages.CountAsync()).Should().Be(0);
        });
    }
}
