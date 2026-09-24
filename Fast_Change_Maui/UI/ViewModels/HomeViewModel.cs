using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.DTOs.Wallets;
using Core.Interfaces;

namespace UI.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    private readonly IUserService _userService;
    private readonly IPortfolioService _portfolioService;

    public ObservableCollection<WalletDto> Wallets { get; } = [];

    [ObservableProperty]
    private string userName = string.Empty;

    [ObservableProperty]
    private decimal totalBalance;

    [ObservableProperty]
    private decimal balanceChange;

    [ObservableProperty]
    private decimal balanceChangePercent;

    [ObservableProperty]
    private bool isBalanceVisible = true;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    public string DisplayBalance => IsBalanceVisible ? TotalBalance.ToString("N2") : "••••••";

    public string DisplayBalanceChange => IsBalanceVisible ? $"{BalanceChange:+0.00;-0.00;0.00}" : "••••••";

    public string DisplayBalanceChangePercent => IsBalanceVisible ? $"{BalanceChangePercent:+0.00;-0.00;0.00}%" : "••••••";

    public bool HasPositiveBalanceChange => BalanceChange > 0;

    public bool HasNegativeBalanceChange => BalanceChange < 0;

    public bool HasNoBalanceChange => BalanceChange == 0;

    public bool HasWallets => Wallets.Count > 0;

    public bool IsErrorVisible => !string.IsNullOrWhiteSpace(ErrorMessage);

    public bool IsEmptyVisible => !IsBusy && !IsErrorVisible && !HasWallets;

    public bool IsContentVisible => !IsBusy && !IsErrorVisible && HasWallets;

    public HomeViewModel(
        IUserService userService,
        IPortfolioService portfolioService)
    {
        _userService = userService;
        _portfolioService = portfolioService;
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            ErrorMessage = string.Empty;

            NotifyStateChanged();

            var userTask = _userService.GetCurrentUserAsync();
            var walletsTask = _userService.GetWalletsAsync();
            var portfolioTask = _portfolioService.GetPortfolioAsync("USD");
            var performanceTask = _portfolioService.GetPerformanceAsync("USD");

            await Task.WhenAll(userTask, walletsTask, portfolioTask, performanceTask);

            var user = await userTask;
            var wallets = await walletsTask;
            var portfolio = await portfolioTask;
            var performance = await performanceTask;

            UserName = user.Email;

            Wallets.Clear();

            foreach (var wallet in wallets)
                Wallets.Add(wallet);

            TotalBalance = portfolio.TotalBalance;

            BalanceChange = performance.ChangeAmount;
            BalanceChangePercent = performance.ChangePercent;

            NotifyStateChanged();
            NotifyBalanceStateChanged();
        }
        catch (Exception)
        {
            ErrorMessage = "Unable to load account data.";
            NotifyStateChanged();
        }
        finally
        {
            IsBusy = false;
            NotifyStateChanged();
        }
    }

    [RelayCommand]
    private void ToggleBalanceVisibility()
    {
        IsBalanceVisible = !IsBalanceVisible;

        NotifyBalanceStateChanged();
    }

    private void NotifyBalanceStateChanged()
    {
        OnPropertyChanged(nameof(DisplayBalance));
        OnPropertyChanged(nameof(DisplayBalanceChange));
        OnPropertyChanged(nameof(DisplayBalanceChangePercent));
        OnPropertyChanged(nameof(HasPositiveBalanceChange));
        OnPropertyChanged(nameof(HasNegativeBalanceChange));
        OnPropertyChanged(nameof(HasNoBalanceChange));
    }

    private void NotifyStateChanged()
    {
        OnPropertyChanged(nameof(HasWallets));
        OnPropertyChanged(nameof(IsErrorVisible));
        OnPropertyChanged(nameof(IsEmptyVisible));
        OnPropertyChanged(nameof(IsContentVisible));
    }
}