using Application.Common.Models;
using Application.Features.Transactions.GetTransactionHistory;
using Contracts.Enums;
using Domain.Entities;
using FluentAssertions;
using IntegrationTests.Authentication;
using IntegrationTests.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Resources;
using System.Net;
using System.Net.Http.Json;

namespace IntegrationTests.Api.Wallet;

public sealed class WalletHistoryTests
    : IntegrationTestBase
{
    public WalletHistoryTests(IntegrationFixture fixture)
        : base(fixture)
    {
    }

    [Fact]
    public async Task GetTransactionHistory_Should_Return_Wallet_Transactions()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var walletId = Guid.NewGuid();

        await ExecuteScopeAsync(async db =>
        {
            var user = new User
            {
                Id = userId,
                Email = "history@test.com",
                PasswordHash = "hash"
            };

            var wallet = new Domain.Entities.Wallet
            {
                Id = walletId,
                UserId = userId,
                Currency = "USD"
            };

            wallet.Deposit(100);

            var transaction1 = Transaction.Create(
                wallet,
                100,
                100,
                wallet.Balance,
                TransactionType.Deposit);

            var transaction2 = Transaction.Create(
                wallet,
                20,
                -20,
                wallet.Balance,
                TransactionType.Transfer);

            db.Users.Add(user);
            db.Wallets.Add(wallet);
            await db.Transactions.AddRangeAsync(transaction1, transaction2);
            await db.SaveChangesAsync();
        });

        TestAuthenticationHandler.UserId = userId;
        TestAuthenticationHandler.Email = "history@test.com";

        // Act
        var response = await Client.GetAsync($"/api/wallet/{walletId}/history");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<PagedResult<GetTransactionHistoryResponse>>();
        result.Should().NotBeNull();
        result!.Items.Should().HaveCount(2);
        result.TotalCount.Should().Be(2);
    }


    [Fact]
    public async Task GetTransactionHistory_Should_Return_Conflict_When_Wallet_Belongs_To_Another_User()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var foreignUserId = Guid.NewGuid();
        var walletId = Guid.NewGuid();

        await ExecuteScopeAsync(async db =>
        {
            var owner = new User
            {
                Id = ownerId,
                Email = "owner@test.com",
                PasswordHash = "hash"
            };

            var foreignUser = new User
            {
                Id = foreignUserId,
                Email = "foreign@test.com",
                PasswordHash = "hash"
            };

            var wallet = new Domain.Entities.Wallet
            {
                Id = walletId,
                UserId = ownerId,
                Currency = "USD"
            };

            db.Users.AddRange(owner, foreignUser);
            db.Wallets.Add(wallet);
            await db.SaveChangesAsync();
        });

        TestAuthenticationHandler.UserId = foreignUserId;
        TestAuthenticationHandler.Email = "foreign@test.com";


        // Act
        var response = await Client.GetAsync($"/api/wallet/{walletId}/history");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);

        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        problem.Should().NotBeNull();
        problem!.Detail.Should().Be(Localization.WalletIsNotAssociatedWithThisUser);
    }
}
