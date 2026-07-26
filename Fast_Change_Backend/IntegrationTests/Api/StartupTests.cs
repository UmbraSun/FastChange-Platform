using FluentAssertions;
using IntegrationTests.Infrastructure;

namespace IntegrationTests.Api;

public sealed class StartupTests : IntegrationTestBase
{
    public StartupTests(IntegrationFixture fixture)
        : base(fixture)
    {
    }

    [Fact]
    public void Application_Should_Start()
    {
        Factory.Should().NotBeNull();
        Client.Should().NotBeNull();
    }
}
