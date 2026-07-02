using Application.Common.Interfaces;
using Persistence.Entities;

namespace Persistence;

public sealed class ProcessedEventStore : IProcessedEventStore
{
    private readonly ApplicationDbContext _db;

    public ProcessedEventStore(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task MarkProcessedAsync(Guid eventId, CancellationToken cancellationToken)
    {
        _db.ProcessedEvents.Add(new ProcessedEvent
        {
            EventId = eventId,
            ProcessedAtUtc = DateTime.UtcNow
        });

        await _db.SaveChangesAsync(cancellationToken);
    }
}
