using Core.DTOs.Transactions;

namespace UI.ViewModels;

public sealed class RecentActivityItemViewModel
{
    public string Type { get; }

    public string Currency { get; }

    public decimal Amount { get; }

    public DateTime CreatedAtUtc { get; }

    public string DisplayAmount =>
        $"{(Amount > 0 ? "+" : string.Empty)}{Amount:N2} {Currency}";

    public string DisplayDate =>
        CreatedAtUtc.ToLocalTime().ToString("dd MMM, HH:mm");

    public bool IsPositive => Amount > 0;

    public bool IsNegative => Amount < 0;

    public bool IsNeutral => Amount == 0;

    public RecentActivityItemViewModel(TransactionDto transaction)
    {
        Type = transaction.Type;
        Currency = transaction.Currency;
        Amount = transaction.Amount;
        CreatedAtUtc = transaction.CreatedAtUtc;
    }
}