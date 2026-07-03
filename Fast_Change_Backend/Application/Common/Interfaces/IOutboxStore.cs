using Application.Common.Models;

namespace Application.Common.Interfaces;

/// <summary>
/// Outbox store interface for managing outbox messages.
/// </summary>
public interface IOutboxStore
{
    /// <summary>
    /// Gets a list of unprocessed outbox messages up to the specified batch size.
    /// </summary>
    /// <param name="batchSize"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task<List<OutboxMessage>> GetUnprocessedAsync(int batchSize, CancellationToken ct);

    /// <summary>
    /// Marks the outbox message with the specified ID as processed, along with the timestamp of processing.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="processedAtUtc"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task MarkAsProcessedAsync(Guid id, DateTime processedAtUtc, CancellationToken ct);
}
