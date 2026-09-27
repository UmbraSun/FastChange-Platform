using UI.ViewModels;

namespace UI.Views.Sections;

public partial class HomeView : ContentView
{
    private readonly HomeViewModel _viewModel;
    private bool _eventsSubscribed;

    public Func<Task>? ExchangeRequested { get; set; }

    public Func<Task>? DepositRequested { get; set; }

    public Func<Task>? WithdrawRequested { get; set; }

    public Func<Task>? TransferRequested { get; set; }

    public Func<Task>? ViewAllWalletsRequested { get; set; }

    public Func<Task>? ProfileRequested { get; set; }

    public HomeView(HomeViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    private void HomeView_Loaded(object? sender, EventArgs e)
    {
        if (_eventsSubscribed)
            return;

        HeaderView.ProfileRequested += OnProfileRequested;

        QuickActionsView.DepositRequested += OnDepositRequested;
        QuickActionsView.ExchangeRequested += OnExchangeRequested;
        QuickActionsView.WithdrawRequested += OnWithdrawRequested;
        QuickActionsView.TransferRequested += OnTransferRequested;

        WalletsPreviewView.ViewAllRequested += OnViewAllWalletsRequested;

        _eventsSubscribed = true;
    }

    private void HomeView_Unloaded(object? sender, EventArgs e)
    {
        if (!_eventsSubscribed)
            return;

        HeaderView.ProfileRequested -= OnProfileRequested;

        QuickActionsView.DepositRequested -= OnDepositRequested;
        QuickActionsView.ExchangeRequested -= OnExchangeRequested;
        QuickActionsView.WithdrawRequested -= OnWithdrawRequested;
        QuickActionsView.TransferRequested -= OnTransferRequested;

        WalletsPreviewView.ViewAllRequested -= OnViewAllWalletsRequested;

        _eventsSubscribed = false;
    }

    public async Task LoadAsync()
    {
        await _viewModel.LoadCommand.ExecuteAsync(null);
    }

    private async void OnProfileRequested(object? sender, EventArgs e)
    {
        if (ProfileRequested is not null)
            await ProfileRequested();
    }

    private async void OnDepositRequested(object? sender, EventArgs e)
    {
        if (DepositRequested is not null)
            await DepositRequested();
    }

    private async void OnExchangeRequested(object? sender, EventArgs e)
    {
        if (ExchangeRequested is not null)
            await ExchangeRequested();
    }

    private async void OnWithdrawRequested(object? sender, EventArgs e)
    {
        if (WithdrawRequested is not null)
            await WithdrawRequested();
    }

    private async void OnTransferRequested(object? sender, EventArgs e)
    {
        if (TransferRequested is not null)
            await TransferRequested();
    }

    private async void OnViewAllWalletsRequested(object? sender, EventArgs e)
    {
        if (ViewAllWalletsRequested is not null)
            await ViewAllWalletsRequested();
    }
}