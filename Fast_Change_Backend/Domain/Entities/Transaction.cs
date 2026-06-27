using Domain.Enums;

namespace Domain.Entities;

public class Transaction
{
    public Guid Id { get; private set; }

    public Guid WalletId { get; private set; }

    public string Currency { get; private set; } = string.Empty;

    public decimal Amount { get; private set; }

    public decimal SignedAmount { get; private set; }

    public decimal BalanceAfter { get; private set; }

    public TransactionType Type { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    public Wallet Wallet { get; private set; } = null!;

    private Transaction() { }

    public static Transaction Create(
        Wallet wallet,
        decimal amount,
        decimal signedAmount,
        TransactionType type)
    {
        return new Transaction
        {
            Id = Guid.NewGuid(),
            WalletId = wallet.Id,
            Currency = wallet.Currency,
            Amount = amount,
            SignedAmount = signedAmount,
            BalanceAfter = wallet.Balance,
            Type = type,
            CreatedAtUtc = DateTime.UtcNow
        };
    }
}
