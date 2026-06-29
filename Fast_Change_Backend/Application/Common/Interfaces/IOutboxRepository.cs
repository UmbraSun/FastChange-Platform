using Application.Common.DTOs;

namespace Application.Common.Interfaces;

public interface IOutboxRepository
{
    Task<List<OutboxMessageDto>> GetUnprocessedAsync(int take, CancellationToken ct);
    Task MarkAsProcessedAsync(Guid messageId, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}
