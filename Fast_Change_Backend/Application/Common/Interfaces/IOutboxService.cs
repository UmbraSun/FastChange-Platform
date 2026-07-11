namespace Application.Common.Interfaces;

/// <summary>
/// Outbox service interface
/// </summary>
public interface IOutboxService
{
    /// <summary>
    /// Publish
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="integrationEvent"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task PublishAsync<T>(
        T integrationEvent,
        CancellationToken cancellationToken);
}
