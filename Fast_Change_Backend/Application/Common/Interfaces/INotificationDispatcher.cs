using Contracts.Events;

namespace Application.Common.Interfaces;

/// <summary>
/// Notification dispatcher interface responsible for dispatching notifications related to various events in the application, such as exchange completion events.
/// </summary>
public interface INotificationDispatcher
{
    /// <summary>
    /// Dispatches an ExchangeCompletedEvent notification to the appropriate handlers or subscribers.
    /// </summary>
    /// <param name="event"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task DispatchExchangeCompletedAsync(
        ExchangeCompletedEvent @event,
        CancellationToken cancellationToken);
}
