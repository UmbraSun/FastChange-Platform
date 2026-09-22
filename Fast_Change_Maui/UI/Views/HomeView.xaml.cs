using UI.ViewModels;

namespace UI.Views.Sections;

public partial class HomeView : ContentView
{
    private readonly HomeViewModel _viewModel;

    public Func<Task>? ExchangeRequested { get; set; }

    public Func<Task>? DepositRequested { get; set; }
    
    public Func<Task>? WithdrawRequested { get; set; }

    public Func<Task>? ViewAllWalletsRequested { get; set; }

    public Func<Task>? ProfileRequested { get; set; }

    public HomeView(HomeViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    public async Task LoadAsync()
    {
        await _viewModel.LoadCommand.ExecuteAsync(null);
    }

    private async void Exchange_Tapped(object? sender, TappedEventArgs e)
    {
        if (ExchangeRequested is not null)
            await ExchangeRequested();
    }

    private async void Deposit_Tapped(object? sender, TappedEventArgs e)
    {
        if (DepositRequested is not null)
            await DepositRequested();
    }

    private async void Withdraw_Tapped(object? sender, TappedEventArgs e)
    {
        if (WithdrawRequested is not null)
            await WithdrawRequested();
    }

    private async void ViewAll_Tapped(object? sender, TappedEventArgs e)
    {
        if (ViewAllWalletsRequested is not null)
            await ViewAllWalletsRequested();
    }

    private async void Profile_Tapped(object? sender, TappedEventArgs e)
    {
        if (ProfileRequested is not null)
            await ProfileRequested();
    }
}