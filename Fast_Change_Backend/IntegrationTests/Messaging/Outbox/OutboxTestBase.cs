using IntegrationTests.Api;
using IntegrationTests.Infrastructure;

namespace IntegrationTests.Messaging.Outbox;

public abstract class OutboxTestBase
    : IntegrationTestBase
{
    protected OutboxTestBase(IntegrationFixture fixture)
        : base(fixture, new OutboxRetryTestFactory(fixture))
    {
    }
}
