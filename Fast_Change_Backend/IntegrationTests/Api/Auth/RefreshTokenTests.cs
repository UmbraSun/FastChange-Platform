using Application.Common.Interfaces;
using Application.Features.Auth.RefreshToken;
using Domain.Entities;
using FluentAssertions;
using IntegrationTests.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;

namespace IntegrationTests.Api.Auth;

public sealed class RefreshTokenTests : IntegrationTestBase
{
    public RefreshTokenTests(IntegrationFixture fixture)
        : base(fixture)
    {
    }

    [Fact]
    public async Task Refresh_Should_Return_New_Tokens_When_Token_Is_Valid()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var refreshTokenGenerator = Factory.Services.GetRequiredService<IJwtTokenGenerator>();

        await ExecuteScopeAsync(async db =>
        {
            db.Users.Add(new User
            {
                Id = userId,
                Email = "refresh@test.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!"),
                IsVerified = true
            });

            await db.SaveChangesAsync();
        });

        var tokens = refreshTokenGenerator.GenerateTokens(userId, "refresh@test.com");
        var command = new RefreshTokenCommand(tokens.RefreshToken);

        // Act
        var response = await Client.PostAsJsonAsync("/api/auth/refresh", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var result = await response.Content.ReadFromJsonAsync<RefreshTokenResponse>();
        result.Should().NotBeNull();
        result!.AccessToken.Should().NotBeNullOrWhiteSpace();
        result.RefreshToken.Should().NotBeNullOrWhiteSpace();
        result.RefreshToken.Should().NotBe(tokens.RefreshToken);
    }


    [Fact]
    public async Task Refresh_Should_Return_Unauthorized_When_Token_Is_Invalid()
    {
        // Arrange
        var command = new RefreshTokenCommand("invalid-refresh-token");

        // Act
        var response = await Client.PostAsJsonAsync("/api/auth/refresh", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
