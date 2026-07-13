namespace Application.Common.Interfaces;

/// <summary>
/// Outbox writer interface for adding events to the outbox.
/// </summary>
public interface IOutboxWriter
{
    /// <summary>
    /// Adds an event to the outbox asynchronously.
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    /// <param name="event"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task AddAsync<TEvent>(TEvent @event, CancellationToken cancellationToken);
}
