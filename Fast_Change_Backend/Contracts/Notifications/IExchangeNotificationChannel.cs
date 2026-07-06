using Contracts.Events;

namespace Contracts.Notifications;

/// <summary>
/// Exchange notification channel interface for sending notifications related to exchange events.
/// </summary>
public interface IExchangeNotificationChannel
{
    /// <summary>
    /// Notifies the channel about an exchange completed event.
    /// </summary>
    /// <param name="event"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task NotifyAsync(
        ExchangeCompletedEvent @event,
        CancellationToken cancellationToken);
}
