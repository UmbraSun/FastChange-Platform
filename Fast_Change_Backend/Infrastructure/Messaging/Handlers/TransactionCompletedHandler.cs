using Application.Common.Interfaces;
using BuildingBlocks.Messaging;
using Contracts.Events;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Messaging.Handlers;

public sealed class TransactionCompletedHandler
    : IIntegrationEventHandler<TransactionCompletedEvent>
{
    private readonly ILogger<TransactionCompletedHandler> _logger;
    private readonly INotificationDispatcher _dispatcher;
    private readonly IWalletHistoryWriter _historyWriter;

    public TransactionCompletedHandler(
        ILogger<TransactionCompletedHandler> logger,
        INotificationDispatcher dispatcher,
        IWalletHistoryWriter historyWriter)
    {
        _logger = logger;
        _dispatcher = dispatcher;
        _historyWriter = historyWriter;
    }

    public async Task HandleAsync(
        TransactionCompletedEvent @event,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Transaction completed: {OperationId}", @event.OperationId);
        await _historyWriter.AddTransactionAsync(@event, cancellationToken);
        await _dispatcher.DispatchTransactionCompletedAsync(@event, cancellationToken);
    }
}
