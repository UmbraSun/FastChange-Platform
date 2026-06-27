using Domain.Enums;

namespace Domain.Entities;

public class Transaction
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid? FromWalletId { get; private set; }

    public Guid? ToWalletId { get; private set; }

    public string FromCurrency { get; private set; } = string.Empty;

    public string ToCurrency { get; private set; } = string.Empty;

    public decimal SentAmount { get; private set; }

    public decimal ReceivedAmount { get; private set; }

    public decimal ExchangeRate { get; private set; }

    public TransactionType Type { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public Wallet? FromWallet { get; set; } = null;

    public Wallet? ToWallet { get; set; } = null;

    private Transaction()
    {
    }

    public static Transaction CreateDeposit(
        Wallet wallet,
        decimal amount)
    {
        return new Transaction
        {
            Id = Guid.NewGuid(),
            ToWalletId = wallet.Id,
            ToCurrency = wallet.Currency,
            ReceivedAmount = amount,
            Type = TransactionType.Deposit,
            CreatedAtUtc = DateTime.UtcNow
        };
    }

    public static Transaction CreateWithdraw(
        Wallet wallet,
        decimal amount)
    {
        return new Transaction
        {
            Id = Guid.NewGuid(),
            FromWalletId = wallet.Id,
            FromCurrency = wallet.Currency,
            SentAmount = amount,
            Type = TransactionType.Withdraw,
            CreatedAtUtc = DateTime.UtcNow
        };
    }

    public static Transaction CreateExchange(
        Wallet fromWallet,
        Wallet toWallet,
        decimal sentAmount,
        decimal receivedAmount,
        decimal exchangeRate)
    {
        return new Transaction
        {
            Id = Guid.NewGuid(),
            FromWalletId = fromWallet.Id,
            ToWalletId = toWallet.Id,

            FromCurrency = fromWallet.Currency,
            ToCurrency = toWallet.Currency,

            SentAmount = sentAmount,
            ReceivedAmount = receivedAmount,
            ExchangeRate = exchangeRate,

            Type = TransactionType.Exchange,
            CreatedAtUtc = DateTime.UtcNow
        };
    }
}
