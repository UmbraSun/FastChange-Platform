using IntegrationTests.Api;
using IntegrationTests.Infrastructure;

namespace IntegrationTests.Messaging.Outbox;

[Collection(nameof(OutboxFailureCollection))]
public abstract class OutboxFailureTestBase
    : IntegrationTestBase
{
    protected OutboxFailureTestBase(IntegrationFixture fixture)
        : base(fixture, new OutboxFailureFactory(fixture))
    {
    }

    protected IntegrationTestFactory CreateFactory(IntegrationFixture fixture)
    {
        return new OutboxFailureTestFactory(fixture);
    }
}
