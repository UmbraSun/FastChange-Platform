using Domain.Enums;

namespace Domain.Entities;

public class Transaction
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid WalletId { get; set; }

    public string Currency { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public TransactionType Type { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public Wallet Wallet { get; set; } = null!;
}
