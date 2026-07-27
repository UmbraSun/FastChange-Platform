using Application.Features.Transfer.TransferFunds;
using Contracts.Enums;
using Contracts.Events;
using Domain.Entities;
using FluentAssertions;
using IntegrationTests.Authentication;
using IntegrationTests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Json;

namespace IntegrationTests.Api.Transfer;

public sealed class TransferTests : IntegrationTestBase
{
    public TransferTests(IntegrationFixture fixture)
        : base(fixture)
    {
    }


    [Fact]
    public async Task Transfer_Should_Move_Funds_And_Create_Transactions()
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
                Email = "transfer@test.com",
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
                UserId = Guid.NewGuid(),
                Currency = "USD"
            };

            db.Users.Add(user);
            db.Wallets.AddRange(fromWallet, toWallet);
            await db.SaveChangesAsync();
        });

        var command = new TransferCommand(fromWalletId, toWalletId, 100);

        // Act
        var response = await Client.PostAsJsonAsync("/api/transfers", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        await ExecuteScopeAsync(async db =>
        {
            var wallets = await db.Wallets.Where(x =>
                x.Id == fromWalletId ||
                x.Id == toWalletId)
            .ToListAsync();

            var transactions = await db.Transactions.ToListAsync();
            var outbox = await db.OutboxMessages.ToListAsync();
            
            var source = wallets.Single(x => x.Id == fromWalletId);
            var destination = wallets.Single(x => x.Id == toWalletId);
            
            source.Balance.Should().Be(900);
            destination.Balance.Should().Be(100);
            transactions.Should().HaveCount(2);

            transactions.Should().Contain(x =>
                x.Type == TransactionType.Transfer &&
                x.SignedAmount == -100);
            transactions.Should().Contain(x =>
                x.Type == TransactionType.Transfer &&
                x.SignedAmount == 100);
            
            outbox.Should().ContainSingle();
            outbox[0].Type.Should().Contain(nameof(TransactionCompletedEvent));
        });
    }

    [Fact]
    public async Task Transfer_Should_Fail_When_Balance_Is_Insufficient()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var receiverId = Guid.NewGuid();

        var fromWalletId = Guid.NewGuid();
        var toWalletId = Guid.NewGuid();

        await ExecuteScopeAsync(async db =>
        {
            var sender = new User
            {
                Id = userId,
                Email = "sender@test.com",
                PasswordHash = "hash",
                IsVerified = true
            };

            var receiver = new User
            {
                Id = receiverId,
                Email = "receiver@test.com",
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
                UserId = receiverId,
                Currency = "USD"
            };

            db.Users.AddRange(sender, receiver);
            db.Wallets.AddRange(fromWallet, toWallet);
            await db.SaveChangesAsync();
        });

        var command = new TransferCommand(fromWalletId, toWalletId, 100);

        // Act
        var response = await Client.PostAsJsonAsync("/api/transfers",command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);

        await ExecuteScopeAsync(async db =>
        {
            var wallets = await db.Wallets.Where(x =>
                x.Id == fromWalletId ||
                x.Id == toWalletId)
            .ToListAsync();

            var transactions = await db.Transactions.ToListAsync();
            var outbox = await db.OutboxMessages.ToListAsync();

            wallets.Single(x => x.Id == fromWalletId).Balance.Should().Be(50);
            wallets.Single(x => x.Id == toWalletId).Balance.Should().Be(0);
            transactions.Should().BeEmpty();
            outbox.Should().BeEmpty();
        });
    }

    [Fact]
    public async Task Transfer_Should_Return_Conflict_When_Source_Wallet_Does_Not_Belong_To_User()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var currentUserId = Guid.NewGuid();
        var receiverId = Guid.NewGuid();

        var fromWalletId = Guid.NewGuid();
        var toWalletId = Guid.NewGuid();

        await ExecuteScopeAsync(async db =>
        {
            var owner = new User
            {
                Id = ownerId,
                Email = "owner@test.com",
                PasswordHash = "hash",
                IsVerified = true
            };

            var currentUser = new User
            {
                Id = currentUserId,
                Email = "current@test.com",
                PasswordHash = "hash",
                IsVerified = true
            };

            var receiver = new User
            {
                Id = receiverId,
                Email = "receiver@test.com",
                PasswordHash = "hash",
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
                UserId = receiverId,
                Currency = "USD"
            };

            db.Users.AddRange(
                owner,
                currentUser,
                receiver);

            db.Wallets.AddRange(
                fromWallet,
                toWallet);

            await db.SaveChangesAsync();
        });

        TestAuthenticationHandler.UserId = currentUserId;
        TestAuthenticationHandler.Email = "current@test.com";

        var command = new TransferCommand(fromWalletId, toWalletId, 100);

        // Act
        var response = await Client.PostAsJsonAsync("/api/transfers", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);

        await ExecuteScopeAsync(async db =>
        {
            var wallets = await db.Wallets.Where(x =>
                x.Id == fromWalletId ||
                x.Id == toWalletId)
            .ToListAsync();

            var transactions = await db.Transactions.ToListAsync();
            var outboxMessages = await db.OutboxMessages.ToListAsync();

            wallets.Single(x => x.Id == fromWalletId).Balance.Should().Be(1000);
            wallets.Single(x => x.Id == toWalletId).Balance.Should().Be(0);
            transactions.Should().BeEmpty();
            outboxMessages.Should().BeEmpty();
        });

        TestAuthenticationHandler.UserId = Guid.Empty;
        TestAuthenticationHandler.Email = string.Empty;
    }

    [Fact]
    public async Task Transfer_Should_Return_Conflict_When_Source_And_Destination_Wallets_Are_The_Same()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var walletId = Guid.NewGuid();

        await ExecuteScopeAsync(async db =>
        {
            var user = new User
            {
                Id = userId,
                Email = "user@test.com",
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

        TestAuthenticationHandler.UserId = userId;
        TestAuthenticationHandler.Email = "user@test.com";

        var command = new TransferCommand(
            walletId,
            walletId,
            100);

        // Act
        var response = await Client.PostAsJsonAsync("/api/transfers", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);

        await ExecuteScopeAsync(async db =>
        {
            var wallet = await db.Wallets.SingleAsync(x => x.Id == walletId);
            var transactions = await db.Transactions.ToListAsync();
            var outboxMessages = await db.OutboxMessages.ToListAsync();

            wallet.Balance.Should().Be(1000);
            transactions.Should().BeEmpty();
            outboxMessages.Should().BeEmpty();
        });

        TestAuthenticationHandler.UserId = Guid.Empty;
        TestAuthenticationHandler.Email = string.Empty;
    }
}
