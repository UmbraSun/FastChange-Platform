using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.DTOs.Wallets;
using Core.Interfaces;
using System.Collections.ObjectModel;

namespace UI.ViewModels;

public partial class DashboardViewModel : ObservableObject
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

    public DashboardViewModel(IUserService userService)
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

            var userTask = _userService.GetCurrentUserAsync();
            var walletsTask = _userService.GetWalletsAsync();

            await Task.WhenAll(userTask, walletsTask);

            var user = await userTask;
            var wallets = await walletsTask;

            UserName = user.Email;

            Wallets.Clear();

            foreach (var wallet in wallets)
                Wallets.Add(wallet);

            TotalBalance = Wallets.Where(x => x.Currency == "USD").Sum(x => x.Balance);
        }
        catch (Exception)
        {
            ErrorMessage = "Unable to load account data.";
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private void ToggleBalanceVisibility()
    {
        IsBalanceVisible = !IsBalanceVisible;
    }
}