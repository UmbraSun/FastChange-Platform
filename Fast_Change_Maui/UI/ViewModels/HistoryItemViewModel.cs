using Core.DTOs.Wallets;

namespace UI.ViewModels;

public sealed class HistoryItemViewModel
{
    public Guid OperationId { get; }

    public Guid WalletId { get; }

    public string Currency { get; }

    public string Type { get; }

    public decimal Amount { get; }

    public decimal BalanceAfter { get; }

    public DateTime CreatedAtUtc { get; }

    public string DisplayAmount => $"{(Amount > 0 ? "+" : string.Empty)}{Amount:N2} {Currency}";

    public string DisplayBalance => $"Balance {BalanceAfter:N2}";

    public string DisplayTime => CreatedAtUtc.ToLocalTime().ToString("h:mm tt");

    public bool IsPositive => Amount > 0;

    public bool IsNegative => Amount < 0;

    public bool IsNeutral => Amount == 0;

    public string Icon => Type switch
    {
        "Deposit" => "↓",
        "Transfer" => "↗",
        "Withdraw" => "↑",
        _ => "•"
    };

    public HistoryItemViewModel(
        WalletHistoryItemDto item,
        string currency)
    {
        OperationId = item.OperationId;
        WalletId = item.WalletId;
        Currency = currency;
        Type = item.OperationType;
        Amount = item.SignedAmount;
        BalanceAfter = item.BalanceAfter;
        CreatedAtUtc = item.CreatedAtUtc;
    }
}