using Application.Features.Auth.LoginUser;
using Application.Features.Exchange.ExchangeCurrency;
using Domain.Entities;
using FluentAssertions;
using IntegrationTests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace IntegrationTests.Api.Exchange;

public sealed class ExchangeNegativeTests
    : IntegrationTestBase
{
    public ExchangeNegativeTests(IntegrationFixture fixture)
        : base(fixture)
    {
    }


    [Fact]
    public async Task Exchange_Should_Return_Conflict_When_Wallet_Does_Not_Exist()
    {
        // Arrange
        var command = new ExchangeCommand(Guid.NewGuid(), Guid.NewGuid(), 100);

        // Act
        var response = await Client.PostAsJsonAsync("/api/exchange", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Exchange_Should_Return_Conflict_When_Balance_Is_Insufficient()
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
                x.Id == toWalletId)
                .ToListAsync();

            wallets.Single(x => x.Id == fromWalletId).Balance.Should().Be(50);
            wallets.Single(x => x.Id == toWalletId).Balance.Should().Be(0);
            db.Transactions.Should().BeEmpty();
            db.OutboxMessages.Should().BeEmpty();
        });
    }

    [Fact]
    public async Task Exchange_Should_Return_Conflict_When_Wallets_Are_Same()
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

            wallet.Deposit(1000);

            db.Users.Add(user);
            db.Wallets.Add(wallet);
            await db.SaveChangesAsync();
        });

        var command = new ExchangeCommand(walletId, walletId, 100);

        // Act
        var response = await Client.PostAsJsonAsync("/api/exchange", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);

        await ExecuteScopeAsync(async db =>
        {
            var wallet = await db.Wallets.SingleAsync(x => x.Id == walletId);
            wallet.Balance.Should().Be(1000);
            db.Transactions.Should().BeEmpty();
            db.OutboxMessages.Should().BeEmpty();
        });
    }

    [Fact]
    public async Task Exchange_Should_Return_Conflict_When_User_Does_Not_Own_Wallet()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var anotherUserId = Guid.NewGuid();
        var fromWalletId = Guid.NewGuid();
        var toWalletId = Guid.NewGuid();

        await ExecuteScopeAsync(async db =>
        {
            var owner = new User
            {
                Id = ownerId,
                Email = "owner@test.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!"),
                IsVerified = true
            };

            var anotherUser = new User
            {
                Id = anotherUserId,
                Email = "another@test.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!"),
                IsVerified = true
            };

            var fromWallet = new Wallet
            {
                Id = fromWalletId,
                UserId = ownerId,
                Currency = "USD"
            };

            fromWallet.Deposit(1000);

            var toWallet = new Wallet
            {
                Id = toWalletId,
                UserId = anotherUserId,
                Currency = "BTC"
            };

            db.Users.AddRange(owner, anotherUser);
            db.Wallets.AddRange(fromWallet, toWallet);
            await db.SaveChangesAsync();
        });


        // Login as another user
        var loginResponse = await Client.PostAsJsonAsync("/api/auth/login", 
            new LoginUserCommand("another@test.com", "Password123!"));
        loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var tokens = await loginResponse.Content.ReadFromJsonAsync<LoginUserResponse>();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens!.AccessToken);

        var command = new ExchangeCommand(fromWalletId, toWalletId, 100);

        // Act
        var response = await Client.PostAsJsonAsync("/api/exchange", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);

        await ExecuteScopeAsync(async db =>
        {
            var wallets = await db.Wallets.Where(x =>
                x.Id == fromWalletId ||
                x.Id == toWalletId)
                .ToListAsync();


            wallets.Single(x => x.Id == fromWalletId).Balance.Should().Be(1000);
            wallets.Single(x => x.Id == toWalletId).Balance.Should().Be(0);
            db.Transactions.Should().BeEmpty();
            db.OutboxMessages.Should().BeEmpty();
        });
    }
}
