using Application.Common.Models;

namespace Application.Common.Interfaces;

/// <summary>
/// Repository interface for managing outbox messages in a database.
/// </summary>
public interface IOutboxRepository
{
    /// <summary>
    /// Gets a list of unprocessed outbox messages from the database, limited by the specified take parameter.
    /// </summary>
    /// <param name="take"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task<List<OutboxMessage>> GetUnprocessedAsync(int take, CancellationToken ct);

    /// <summary>
    /// Marks the specified outbox message as processed in the database.
    /// </summary>
    /// <param name="messageId"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task MarkAsProcessedAsync(Guid messageId, CancellationToken ct);
}
