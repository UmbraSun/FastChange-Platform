using Application.Features.Exchange.ExchangeCurrency;
using FluentAssertions;
using IntegrationTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;

namespace IntegrationTests.Api.Auth;

public sealed class AuthorizationTests : IntegrationTestBase
{
    public AuthorizationTests(IntegrationFixture fixture)
        : base(fixture)
    {
    }

    [Fact]
    public async Task Exchange_Should_Return_Unauthorized_Without_Token()
    {
        // Arrange
        Client.DefaultRequestHeaders.Authorization = null;

        var command = new ExchangeCommand(Guid.NewGuid(), Guid.NewGuid(), 100);

        // Act
        var response = await Client.PostAsJsonAsync("/api/exchange", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
