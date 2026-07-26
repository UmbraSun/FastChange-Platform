using FluentAssertions;
using IntegrationTests.Infrastructure;
using System.Net;

namespace IntegrationTests.Api.Health;

public sealed class HealthTests : IntegrationTestBase
{
    public HealthTests(IntegrationFixture fixture)
        : base(fixture)
    {
    }

    [Fact]
    public async Task Health_Should_Return_Ok()
    {
        // Act
        var response = await Client.GetAsync("/health");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
