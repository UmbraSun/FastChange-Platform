using Application.Common.Interfaces;
using BuildingBlocks.Messaging;
using Contracts.Events;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Messaging.Handlers;

public sealed class ExchangeCompletedHandler
    : IIntegrationEventHandler<ExchangeCompletedEvent>
{
    private readonly ILogger<ExchangeCompletedHandler> _logger;
    private readonly INotificationDispatcher _dispatcher;
    private readonly IWalletHistoryWriter _historyWriter;

    public ExchangeCompletedHandler(
        ILogger<ExchangeCompletedHandler> logger,
        INotificationDispatcher dispatcher,
        IWalletHistoryWriter historyWriter)
    {
        _logger = logger;
        _dispatcher = dispatcher;
        _historyWriter = historyWriter;
    }

    public async Task HandleAsync(
        ExchangeCompletedEvent @event,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Exchange completed: {OperationId}",
            @event.OperationId);

        await _historyWriter.AddExchangeAsync(
            @event,
            cancellationToken);

        await _dispatcher.DispatchExchangeCompletedAsync(
            @event,
            cancellationToken);
    }
}
