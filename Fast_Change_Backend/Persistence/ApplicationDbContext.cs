using Application.Common.Models;
using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Entities;
using System.Text.Json;

namespace Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Domain core
    public DbSet<User> Users => Set<User>();
    public DbSet<Wallet> Wallets => Set<Wallet>();
    public DbSet<Transaction> Transactions => Set<Transaction>();

    // Integration / messaging persistence
    public DbSet<ProcessedEvent> ProcessedEvents => Set<ProcessedEvent>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Highload optimization: Discovers and applies all IEntityTypeConfiguration classes 
        // located in this assembly to keep the DbContext file clean and modular.
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEvents = ChangeTracker
            .Entries<IHasDomainEvents>()
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        var outboxMessages = domainEvents.Select(e => new OutboxMessage
        {
            Id = Guid.NewGuid(),
            Type = e.GetType().Name,
            Payload = JsonSerializer.Serialize(e),
            OccurredOnUtc = e.OccurredOnUtc
        });

        AddRange(outboxMessages);

        foreach (var entity in ChangeTracker.Entries<IHasDomainEvents>())
            entity.Entity.ClearDomainEvents();

        return await base.SaveChangesAsync(cancellationToken);
    }
}
