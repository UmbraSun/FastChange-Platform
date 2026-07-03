using Application.Common.Interfaces;
using Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Outbox;

public sealed class OutboxStore : IOutboxStore
{
    private readonly ApplicationDbContext _db;

    public OutboxStore(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<List<OutboxMessage>> GetUnprocessedAsync(int batchSize, CancellationToken ct)
    {
        return await _db.OutboxMessages
            .Where(x => x.ProcessedOnUtc == null)
            .OrderBy(x => x.OccurredOnUtc)
            .Take(batchSize)
            .ToListAsync(ct);
    }

    public async Task MarkAsProcessedAsync(Guid id, DateTime processedAtUtc, CancellationToken ct)
    {
        var message = await _db.OutboxMessages
            .FirstAsync(x => x.Id == id, ct);

        message.ProcessedOnUtc = processedAtUtc;

        await _db.SaveChangesAsync(ct);
    }
}
