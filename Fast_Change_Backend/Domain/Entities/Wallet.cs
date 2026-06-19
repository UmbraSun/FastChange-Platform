namespace Domain.Entities;

public class Wallet
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public string Currency { get; set; } = string.Empty;
    public decimal Balance { get; set; }

    // Concurrency token for Optimistic Locking under high concurrent write loads
    public uint Version { get; set; }

    // Navigation property
    public User User { get; set; } = null!;
}
