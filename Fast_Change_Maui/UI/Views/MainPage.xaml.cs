using UI.ViewModels;

namespace UI.Views;

public partial class MainPage : ContentPage
{
    private readonly MainViewModel _viewModel;

    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;
        BindingContext = viewModel;

        BottomNavigation.HomeSelected += OnHomeSelected;
        BottomNavigation.WalletsSelected += OnWalletsSelected;
        BottomNavigation.ExchangeSelected += OnExchangeSelected;
        BottomNavigation.HistorySelected += OnHistorySelected;
        BottomNavigation.ProfileSelected += OnProfileSelected;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _viewModel.LoadHomeCommand.ExecuteAsync(null);
    }

    private void OnHomeSelected(object? sender, EventArgs e)
    {
        _viewModel.ShowHome();
    }

    private void OnWalletsSelected(object? sender, EventArgs e)
    {
        _viewModel.ShowWallets();
    }

    private async void OnExchangeSelected(object? sender, EventArgs e)
    {
        await _viewModel.ShowExchangeAsync();
    }

    private async void OnHistorySelected(object? sender, EventArgs e)
    {
        await _viewModel.ShowHistoryAsync();
    }

    private async void OnProfileSelected(object? sender, EventArgs e)
    {
        await _viewModel.ShowProfileAsync();
    }
}