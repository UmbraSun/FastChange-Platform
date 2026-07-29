using Domain.Entities;
using FluentAssertions;
using IntegrationTests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

namespace IntegrationTests.Api;

public sealed class OptimisticConcurrencyTests
    : IntegrationTestBase
{
    public OptimisticConcurrencyTests(IntegrationFixture fixture)
        : base(fixture)
    {
    }

    [Fact]
    public async Task Wallet_Should_Throw_Concurrency_Exception_When_Updated_Concurrently()
    {
        // Arrange
        var walletId = Guid.NewGuid();

        await ExecuteScopeAsync(async db =>
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = "test@test.com",
                PasswordHash = "hash"
            };

            var wallet = new Domain.Entities.Wallet
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

        await using var scope1 = Factory.Services.CreateAsyncScope();
        await using var scope2 = Factory.Services.CreateAsyncScope();

        var db1 = scope1.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var db2 = scope2.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var wallet1 = await db1.Wallets.SingleAsync(x => x.Id == walletId);
        var wallet2 = await db2.Wallets.SingleAsync(x => x.Id == walletId);

        // Act
        wallet1.Deposit(10);
        await db1.SaveChangesAsync();

        wallet2.Deposit(20);

        // Assert
        await FluentActions.Invoking(() => db2.SaveChangesAsync()).Should().ThrowAsync<DbUpdateConcurrencyException>();
    }
}
