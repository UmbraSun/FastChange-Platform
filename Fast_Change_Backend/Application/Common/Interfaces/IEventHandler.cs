namespace Application.Common.Interfaces;

/// <summary>
/// Event handler interface for handling events of type <typeparamref name="TEvent"/>.
/// </summary>
/// <typeparam name="TEvent"></typeparam>
public interface IEventHandler<in TEvent>
{
    /// <summary>
    /// Handles the specified event asynchronously.
    /// </summary>
    /// <param name="event"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task HandleAsync(TEvent @event, CancellationToken cancellationToken);
}
