using Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Features.Exchange.Events;

public sealed class ExchangeCompletedHandler
    : IEventHandler<ExchangeCompletedEvent>
{
    private readonly ILogger<ExchangeCompletedHandler> _logger;

    public ExchangeCompletedHandler(ILogger<ExchangeCompletedHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(
        ExchangeCompletedEvent @event,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Exchange completed: {OperationId}",
            @event.OperationId);

        return Task.CompletedTask;
    }
}
