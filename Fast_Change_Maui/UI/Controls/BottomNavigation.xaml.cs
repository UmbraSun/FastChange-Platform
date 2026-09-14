namespace UI.Controls;

public partial class BottomNavigation : ContentView
{
    public event EventHandler? HomeSelected;
    public event EventHandler? WalletsSelected;
    public event EventHandler? ExchangeSelected;
    public event EventHandler? HistorySelected;
    public event EventHandler? ProfileSelected;

    public BottomNavigation()
    {
        InitializeComponent();
    }

    private void Home_Tapped(object? sender, TappedEventArgs e)
    {
        HomeSelected?.Invoke(this, EventArgs.Empty);
    }

    private void Wallets_Tapped(object? sender, TappedEventArgs e)
    {
        WalletsSelected?.Invoke(this, EventArgs.Empty);
    }

    private void Exchange_Tapped(object? sender, TappedEventArgs e)
    {
        ExchangeSelected?.Invoke(this, EventArgs.Empty);
    }

    private void History_Tapped(object? sender, TappedEventArgs e)
    {
        HistorySelected?.Invoke(this, EventArgs.Empty);
    }

    private void Profile_Tapped(object? sender, TappedEventArgs e)
    {
        ProfileSelected?.Invoke(this, EventArgs.Empty);
    }
}