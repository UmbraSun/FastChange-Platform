using Resources;

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
                Localization.AmountGreaterThanZero);

        Balance += amount;
    }

    public void Withdraw(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(amount),
                Localization.AmountGreaterThanZero);

        if (Balance < amount)
            throw new InvalidOperationException(
                Localization.InsufficientFunds);

        Balance -= amount;
    }

    public ICollection<Transaction> Transactions { get; set; }
        = new List<Transaction>();
}
