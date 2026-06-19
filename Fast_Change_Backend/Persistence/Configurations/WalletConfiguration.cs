using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class WalletConfiguration : IEntityTypeConfiguration<Wallet>
{
    public void Configure(EntityTypeBuilder<Wallet> builder)
    {
        builder.ToTable("wallets");

        builder.HasKey(w => w.Id);

        // Highload optimization: Prevents a single user from opening duplicate wallets for the same asset.
        builder.HasIndex(w => new { w.UserId, w.Currency })
            .IsUnique();

        builder.Property(w => w.Currency)
            .HasMaxLength(10)
            .IsRequired();

        // Fintech scale configuration: 18 digits total, 4 digits reserved for precise fiat/crypto fractions
        builder.Property(w => w.Balance)
            .HasPrecision(18, 4)
            .HasDefaultValue(0.0000m);

        // Highload Concurrency Optimization: Instructs EF Core to evaluate the 'Version' field 
        // as an optimistic locking trigger during balance writes.
        builder.Property(w => w.Version)
            .IsRowVersion();

        builder.HasOne(w => w.User)
            .WithMany(u => u.Wallets)
            .HasForeignKey(w => w.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
