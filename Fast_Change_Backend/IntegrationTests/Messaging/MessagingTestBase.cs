using IntegrationTests.Api;
using IntegrationTests.Infrastructure;

namespace IntegrationTests.Messaging;

public abstract class MessagingTestBase
    : IntegrationTestBase
{
    protected readonly IServiceProvider Services;

    protected MessagingTestBase(IntegrationFixture fixture)
        : base(fixture, new MessagingTestFactory(fixture))
    {
        Services = Factory.Services;
    }
}
