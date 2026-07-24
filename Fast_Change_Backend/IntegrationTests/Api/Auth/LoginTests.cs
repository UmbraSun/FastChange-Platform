using Application.Features.Auth.LoginUser;
using Domain.Entities;
using FluentAssertions;
using IntegrationTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;

namespace IntegrationTests.Api.Auth;

public sealed class LoginTests : IntegrationTestBase
{
    public LoginTests(IntegrationFixture fixture)
        : base(fixture)
    {
    }

    [Fact]
    public async Task Login_Should_Return_Tokens_When_Credentials_Are_Valid()
    {
        // Arrange
        var command = new LoginUserCommand("login@test.com", "Password123!");

        await ExecuteScopeAsync(async db =>
        {
            db.Users.Add(new User
            {
                Id = Guid.NewGuid(),
                Email = command.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(command.Password),
                IsVerified = true
            });

            await db.SaveChangesAsync();
        });

        // Act
        var response = await Client.PostAsJsonAsync("/api/auth/login", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var result = await response.Content.ReadFromJsonAsync<LoginUserResponse>();
        result.Should().NotBeNull();
        result!.AccessToken.Should().NotBeNullOrWhiteSpace();
        result.RefreshToken.Should().NotBeNullOrWhiteSpace();
    }


    [Fact]
    public async Task Login_Should_Return_Unauthorized_When_Password_Is_Invalid()
    {
        // Arrange
        var command = new LoginUserCommand("invalid@test.com", "WrongPassword123!");

        await ExecuteScopeAsync(async db =>
        {
            db.Users.Add(new User
            {
                Id = Guid.NewGuid(),
                Email = command.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!"),
                IsVerified = true
            });

            await db.SaveChangesAsync();
        });

        // Act
        var response = await Client.PostAsJsonAsync("/api/auth/login", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }


    [Fact]
    public async Task Login_Should_Return_Unauthorized_When_User_Does_Not_Exist()
    {
        // Arrange
        var command = new LoginUserCommand("unknown@test.com", "Password123!");

        // Act
        var response = await Client.PostAsJsonAsync("/api/auth/login", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
