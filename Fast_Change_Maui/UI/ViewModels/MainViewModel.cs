using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using UI.Views.Sections;

namespace UI.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly HomeView _homeView;
    private readonly WalletsView _walletsView;
    private readonly ExchangeView _exchangeView;
    private readonly HistoryView _historyView;

    [ObservableProperty]
    private ContentView currentView;

    public MainViewModel(
        HomeView homeView,
        WalletsView walletsView,
        ExchangeView exchangeView,
        HistoryView historyView)
    {
        _homeView = homeView;
        _walletsView = walletsView;
        _exchangeView = exchangeView;
        _historyView = historyView;

        CurrentView = _homeView;
    }

    [RelayCommand]
    private Task LoadHomeAsync()
    {
        return _homeView.LoadAsync();
    }

    [RelayCommand]
    private Task LoadHistoryAsync()
    {
        return _historyView.LoadAsync();
    }

    public void ShowHome()
    {
        CurrentView = _homeView;
    }

    public void ShowWallets()
    {
        CurrentView = _walletsView;
    }

    public void ShowExchange()
    {
        CurrentView = _exchangeView;
    }

    public async Task ShowHistoryAsync()
    {
        CurrentView = _historyView;

        await LoadHistoryAsync();
    }
}
