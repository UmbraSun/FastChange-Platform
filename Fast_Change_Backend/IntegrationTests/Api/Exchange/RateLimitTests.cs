using Application.Features.Exchange.ExchangeCurrency;
using FluentAssertions;
using IntegrationTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;

namespace IntegrationTests.Api.Exchange;

public sealed class RateLimitTests : IntegrationTestBase
{
    public RateLimitTests(IntegrationFixture fixture)
        : base(fixture)
    {
    }

    [Fact]
    public async Task Exchange_Should_Return_TooManyRequests_When_Limit_Exceeded()
    {
        // Arrange
        var command = new ExchangeCommand(Guid.NewGuid(), Guid.NewGuid(), 100);

        HttpResponseMessage? lastResponse = null;

        // Act
        for (var i = 0; i < 20; i++)
        {
            lastResponse = await Client.PostAsJsonAsync("/api/exchange", command);

            if (lastResponse.StatusCode == HttpStatusCode.TooManyRequests) break;
        }

        // Assert
        lastResponse.Should().NotBeNull();
        lastResponse!.StatusCode.Should().Be(HttpStatusCode.TooManyRequests);
    }
}
