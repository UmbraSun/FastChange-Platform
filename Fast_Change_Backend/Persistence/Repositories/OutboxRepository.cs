using Application.Common.Interfaces;
using Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class OutboxRepository : IOutboxRepository
{
    private readonly ApplicationDbContext _db;

    public OutboxRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<List<OutboxMessage>> GetUnprocessedAsync(int take, CancellationToken ct)
    {
        var entities = await _db.Set<OutboxMessage>()  
            .Where(x => x.ProcessedOnUtc == null)
            .OrderBy(x => x.OccurredOnUtc)
            .Take(take)
            .ToListAsync(ct);

        return entities.Select(x => new OutboxMessage
        {
            Id = x.Id,
            Type = x.Type,
            Payload = x.Payload,
            OccurredOnUtc = x.OccurredOnUtc,
            ProcessedOnUtc = x.ProcessedOnUtc
        }).ToList();
    }

    public async Task MarkAsProcessedAsync(Guid messageId, CancellationToken ct)
    {
        var entity = await _db.Set<OutboxMessage>().FindAsync(new object[] { messageId }, ct);
        if (entity != null)
        {
            entity.ProcessedOnUtc = DateTime.UtcNow;
        }
    }

    public Task SaveChangesAsync(CancellationToken ct)
    {
        return _db.SaveChangesAsync(ct);
    }
}
