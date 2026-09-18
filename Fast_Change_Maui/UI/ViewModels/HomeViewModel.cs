using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.DTOs.Wallets;
using Core.Interfaces;

namespace UI.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    private readonly IUserService _userService;

    public ObservableCollection<WalletDto> Wallets { get; } = [];

    [ObservableProperty]
    private string userName = string.Empty;

    [ObservableProperty]
    private decimal totalBalance;

    [ObservableProperty]
    private bool isBalanceVisible = true;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    public string DisplayBalance => IsBalanceVisible ? TotalBalance.ToString("N2") : "••••••";

    public bool HasWallets => Wallets.Count > 0;

    public bool IsErrorVisible => !string.IsNullOrWhiteSpace(ErrorMessage);

    public bool IsEmptyVisible =>
        !IsBusy &&
        !IsErrorVisible &&
        !HasWallets;

    public bool IsContentVisible =>
        !IsBusy &&
        !IsErrorVisible &&
        HasWallets;

    public HomeViewModel(IUserService userService)
    {
        _userService = userService;
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

            await Task.WhenAll(userTask, walletsTask);

            var user = await userTask;
            var wallets = await walletsTask;

            UserName = user.Email;
            Wallets.Clear();

            foreach (var wallet in wallets)
                Wallets.Add(wallet);

            TotalBalance = Wallets
                .Where(wallet => wallet.Currency == "USD")
                .Sum(wallet => wallet.Balance);

            OnPropertyChanged(nameof(DisplayBalance));
            NotifyStateChanged();
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

        OnPropertyChanged(nameof(DisplayBalance));
    }

    private void NotifyStateChanged()
    {
        OnPropertyChanged(nameof(HasWallets));
        OnPropertyChanged(nameof(IsErrorVisible));
        OnPropertyChanged(nameof(IsEmptyVisible));
        OnPropertyChanged(nameof(IsContentVisible));
    }
}