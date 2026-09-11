using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.DTOs.Wallets;
using Core.Interfaces;
using System.Collections.ObjectModel;

namespace UI.ViewModels;

public partial class WalletsViewModel : ObservableObject
{
    private readonly IUserService _userService;

    public ObservableCollection<WalletDto> Wallets { get; } = [];

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    public WalletsViewModel(IUserService userService)
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

            var wallets = await _userService.GetWalletsAsync();

            Wallets.Clear();

            foreach (var wallet in wallets)
            {
                Wallets.Add(wallet);
            }
        }
        catch (Exception)
        {
            ErrorMessage = "Unable to load wallets.";
        }
        finally
        {
            IsBusy = false;
        }
    }
}