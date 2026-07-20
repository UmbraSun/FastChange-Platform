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

    public Task AddAsync(
        OutboxMessage message,
        CancellationToken ct)
    {
        return _db.OutboxMessages
            .AddAsync(message, ct)
            .AsTask();
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
            Topic = x.Topic,
            Key = x.Key,
            OccurredOnUtc = x.OccurredOnUtc,
            ProcessedOnUtc = x.ProcessedOnUtc
        }).ToList();
    }

    public async Task MarkAsProcessedAsync(Guid messageId, CancellationToken ct)
    {
        var entity = await _db.Set<OutboxMessage>().FindAsync([messageId], ct);
        entity?.ProcessedOnUtc = DateTime.UtcNow;
    }
}
