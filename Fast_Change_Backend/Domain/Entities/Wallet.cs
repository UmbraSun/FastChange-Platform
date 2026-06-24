namespace Domain.Entities;

public class Wallet
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public string Currency { get; set; } = string.Empty;
    public decimal Balance { get; private set; }

    // Concurrency token for Optimistic Locking under high concurrent write loads
    public uint Version { get; set; }

    // Navigation property
    public User User { get; set; } = null!;

    public void Deposit(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(amount),
                "Amount must be greater than zero.");

        Balance += amount;
    }

    public void Withdraw(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(amount),
                "Amount must be greater than zero.");

        if (Balance < amount)
            throw new InvalidOperationException(
                "Insufficient funds.");

        Balance -= amount;
    }
}
