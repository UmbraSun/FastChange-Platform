namespace UI.ViewModels;

public sealed class HistoryWalletFilterViewModel
{
    public Guid? WalletId { get; }

    public string DisplayName { get; }

    public HistoryWalletFilterViewModel(
        Guid? walletId,
        string displayName)
    {
        WalletId = walletId;
        DisplayName = displayName;
    }
}