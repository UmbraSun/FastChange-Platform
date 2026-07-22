using IntegrationTests.Api;
using IntegrationTests.Infrastructure;

namespace IntegrationTests.Messaging;

[Collection(nameof(MessagingCollection))]
public abstract class MessagingTestBase
    : IntegrationTestBase
{
    protected readonly IServiceProvider Services;

    protected MessagingTestBase(IntegrationFixture fixture)
        : base(fixture)
    {
        Factory.CreateClient();
        Services = Factory.Services;
    }
}
