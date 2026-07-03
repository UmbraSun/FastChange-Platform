using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence.Entities;

namespace Persistence;

public sealed class ProcessedEventStore : IProcessedEventStore
{
    private readonly ApplicationDbContext _db;

    public ProcessedEventStore(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<bool> TryMarkProcessedAsync(Guid eventId, CancellationToken ct)
    {
        var exists = await _db.ProcessedEvents
            .AnyAsync(x => x.EventId == eventId, ct);

        if (exists)
            return false;

        _db.ProcessedEvents.Add(new ProcessedEvent
        {
            EventId = eventId,
            ProcessedAtUtc = DateTime.UtcNow
        });

        await _db.SaveChangesAsync(ct);
        return true;
    }
}
