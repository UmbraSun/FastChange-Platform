using Application.Common.Models;

namespace Application.Common.Interfaces;

public interface IOutboxRepository
{
    Task<List<OutboxMessage>> GetUnprocessedAsync(int take, CancellationToken ct);
    Task MarkAsProcessedAsync(Guid messageId, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}
