namespace Application.Common.Interfaces;

/// <summary>
/// Event bus interface for publishing messages to a message broker or event streaming platform.
/// </summary>
public interface IEventBus
{
    /// <summary>
    /// Publishes an event to the specified topic with a given key.
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="topic"></param>
    /// <param name="key"></param>
    /// <param name="event"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task PublishAsync<T>(
        Guid eventId,
        string topic,
        string key,
        T @event,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Publishes a raw payload to the specified topic with a given key.
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="topic"></param>
    /// <param name="key"></param>
    /// <param name="payload"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task PublishAsync(
        Guid eventId,
        string topic,
        string key,
        string payload,
        CancellationToken cancellationToken = default);
}
