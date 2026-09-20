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
    private readonly ProfileView _profileView;
    private readonly DepositView _depositView;

    [ObservableProperty]
    private ContentView currentView;

    public MainViewModel(
        HomeView homeView,
        WalletsView walletsView,
        ExchangeView exchangeView,
        HistoryView historyView,
        ProfileView profileView,
        DepositView depositView)
    {
        _homeView = homeView;
        _walletsView = walletsView;
        _exchangeView = exchangeView;
        _historyView = historyView;
        _profileView = profileView;
        _depositView = depositView;

        _homeView.ExchangeRequested = ShowExchangeAsync;
        _homeView.DepositRequested = ShowDepositAsync;
        _homeView.ViewAllWalletsRequested = ShowWalletsAsync;
        _homeView.ProfileRequested = ShowProfileAsync;

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

    [RelayCommand]
    private Task LoadProfileAsync()
    {
        return _profileView.LoadAsync();
    }

    public void ShowHome()
    {
        CurrentView = _homeView;
    }

    public async Task ShowWalletsAsync()
    {
        CurrentView = _walletsView;
        await _walletsView.LoadAsync();
    }

    public async Task ShowExchangeAsync()
    {
        CurrentView = _exchangeView;
        await _exchangeView.LoadAsync();
    }

    public async Task ShowHistoryAsync()
    {
        CurrentView = _historyView;
        await LoadHistoryAsync();
    }

    public async Task ShowProfileAsync()
    {
        CurrentView = _profileView;
        await LoadProfileAsync();
    }

    public async Task ShowDepositAsync()
    {
        CurrentView = _depositView;
        await _depositView.LoadAsync();
    }
}