namespace UI.Views.Sections.Home;

public partial class QuickActionsView : ContentView
{
    public event EventHandler? DepositRequested;
    public event EventHandler? ExchangeRequested;
    public event EventHandler? WithdrawRequested;
    public event EventHandler? TransferRequested;

    public QuickActionsView()
    {
        InitializeComponent();
    }

    private void Deposit_Tapped(object? sender, TappedEventArgs e)
    {
        DepositRequested?.Invoke(this, EventArgs.Empty);
    }

    private void Exchange_Tapped(object? sender, TappedEventArgs e)
    {
        ExchangeRequested?.Invoke(this, EventArgs.Empty);
    }

    private void Withdraw_Tapped(object? sender, TappedEventArgs e)
    {
        WithdrawRequested?.Invoke(this, EventArgs.Empty);
    }

    private void Transfer_Tapped(object? sender, TappedEventArgs e)
    {
        TransferRequested?.Invoke(this, EventArgs.Empty);
    }
}