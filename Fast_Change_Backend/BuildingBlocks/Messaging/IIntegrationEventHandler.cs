namespace BuildingBlocks.Messaging;

/// <summary>
/// Interface for handling integration events. Implement this interface to create a handler for a specific integration event type.
/// </summary>
/// <typeparam name="TEvent"></typeparam>
public interface IIntegrationEventHandler<in TEvent>
{
    /// <summary>
    /// Handles the specified integration event asynchronously.
    /// </summary>
    /// <param name="event"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task HandleAsync(TEvent @event, CancellationToken cancellationToken);
}
