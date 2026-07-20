using BuildingBlocks.Messaging;
using Contracts.Events;

namespace IntegrationTests.Infrastructure.Fakes;

public sealed class FakeExchangeCompletedHandler
    : IIntegrationEventHandler<ExchangeCompletedEvent>
{
    public Task HandleAsync(
        ExchangeCompletedEvent @event,
        CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
