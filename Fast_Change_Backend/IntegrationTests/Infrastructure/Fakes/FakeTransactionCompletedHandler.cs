using BuildingBlocks.Messaging;
using Contracts.Events;

namespace IntegrationTests.Infrastructure.Fakes;

public sealed class FakeTransactionCompletedHandler
    : IIntegrationEventHandler<TransactionCompletedEvent>
{
    public Task HandleAsync(
        TransactionCompletedEvent @event,
        CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
