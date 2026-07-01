using Domain.Entities;

namespace Application.Common.Interfaces;

/// <summary>
/// Provides an interface for a repository that manages processed events, allowing for checking existence and adding new processed events.
/// </summary>
public interface IProcessedEventRepository
{
    /// <summary>
    /// ExistsAsync checks if a processed event with the specified eventId exists in the repository.
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task<bool> ExistsAsync(Guid eventId, CancellationToken ct);

    /// <summary>
    /// AddAsync adds a new processed event to the repository.
    /// </summary>
    /// <param name="processedEvent"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task AddAsync(ProcessedEvent processedEvent, CancellationToken ct);
}
