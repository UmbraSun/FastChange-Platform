using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.DTOs.Wallets;
using Core.Interfaces;

namespace UI.ViewModels;

public partial class WalletsViewModel : ObservableObject
{
    private readonly IUserService _userService;

    public ObservableCollection<WalletDto> Wallets { get; } = [];

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string errorMessage = string.Empty;

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

            OnPropertyChanged(nameof(IsErrorVisible));
            OnPropertyChanged(nameof(IsEmptyVisible));
            OnPropertyChanged(nameof(IsContentVisible));

            var wallets = await _userService.GetWalletsAsync();

            Wallets.Clear();

            foreach (var wallet in wallets)
                Wallets.Add(wallet);

            OnPropertyChanged(nameof(HasWallets));
        }
        catch (Exception)
        {
            ErrorMessage = "Unable to load wallets.";
            OnPropertyChanged(nameof(IsErrorVisible));
        }
        finally
        {
            IsBusy = false;

            OnPropertyChanged(nameof(IsEmptyVisible));
            OnPropertyChanged(nameof(IsContentVisible));
        }
    }
}