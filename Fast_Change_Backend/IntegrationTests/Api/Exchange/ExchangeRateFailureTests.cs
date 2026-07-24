using Application.Features.Exchange.ExchangeCurrency;
using Domain.Entities;
using FluentAssertions;
using IntegrationTests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Json;

namespace IntegrationTests.Api.Exchange;

[Collection(nameof(ExchangeRateCollection))]
public sealed class ExchangeRateFailureTests
    : IntegrationTestBase
{
    private readonly ExchangeRateFailureFactory _factory;

    public ExchangeRateFailureTests(IntegrationFixture fixture, ExchangeRateFailureFactory factory)
        : base(fixture)
    {
        _factory = factory;
    }


    [Fact]
    public async Task Exchange_Should_Return_Conflict_When_Exchange_Rate_Not_Found()
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
                Email = "rate@test.com",
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
