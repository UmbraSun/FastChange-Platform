using FastChange.Application.Features.Users.RegisterUser;
using FluentAssertions;
using IntegrationTests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Json;

namespace IntegrationTests.Api.Auth;

public sealed class RegisterTests : IntegrationTestBase
{
    public RegisterTests(IntegrationFixture fixture)
        : base(fixture)
    {
    }

    [Fact]
    public async Task Register_Should_Create_User_And_Default_Wallets()
    {
        // Arrange
        var command = new RegisterUserCommand("john@test.com", "Password123!");

        // Act
        var response = await Client.PostAsJsonAsync("/api/auth/register", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await ExecuteScopeAsync(async db =>
        {
            var user = await db.Users
                .Include(x => x.Wallets)
                .SingleAsync(x => x.Email == command.Email);

            user.Should().NotBeNull();
            user.PasswordHash.Should().NotBe(command.Password);
            user.PasswordHash.Should().NotBeNullOrWhiteSpace();
            user.Wallets.Should().NotBeEmpty();
        });
    }

    [Fact]
    public async Task Register_Should_Return_Conflict_When_Email_Already_Exists()
    {
        // Arrange
        var command = new RegisterUserCommand("duplicate@test.com", "Password123!");

        // Act
        var first = await Client.PostAsJsonAsync("/api/auth/register", command);

        // Assert
        first.StatusCode.Should().Be(HttpStatusCode.OK);

        var second = await Client.PostAsJsonAsync("/api/auth/register", command);
        second.StatusCode.Should().Be(HttpStatusCode.Conflict);

        await ExecuteScopeAsync(async db =>
        {
            var count = await db.Users.CountAsync(x => x.Email == command.Email);
            count.Should().Be(1);
        });
    }
}
