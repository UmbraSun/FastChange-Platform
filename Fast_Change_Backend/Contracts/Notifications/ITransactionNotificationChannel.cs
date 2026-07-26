using Contracts.Events;

namespace Contracts.Notifications;

/// <summary>
/// Transaction notification channel interface for sending notifications related to transaction events.
/// </summary>
public interface ITransactionNotificationChannel
{
    /// <summary>
    /// Notifies the channel about a transaction completed event.
    /// </summary>
    /// <param name="event"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task NotifyAsync(
        TransactionCompletedEvent @event,
        CancellationToken cancellationToken);
}
