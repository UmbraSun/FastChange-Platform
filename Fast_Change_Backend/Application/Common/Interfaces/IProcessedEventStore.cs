namespace Application.Common.Interfaces;

/// <summary>
/// Provides an interface for storing and checking processed events to ensure idempotency in event handling.
/// </summary>
public interface IProcessedEventStore
{
    /// <summary>
    /// Marks an event as processed by storing its unique identifier in the event store.
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task MarkProcessedAsync(
        Guid eventId,
        CancellationToken cancellationToken);
}
