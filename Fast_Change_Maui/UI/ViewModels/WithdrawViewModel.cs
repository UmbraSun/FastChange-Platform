using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.DTOs.Wallets;
using Core.Interfaces;
using IntelliJ.Lang.Annotations;
using Refit;
using System.Collections.ObjectModel;
using System.Net;

namespace UI.ViewModels;

public partial class WithdrawViewModel : ObservableObject
{
    private readonly IUserService _userService;
    private readonly IWalletOperationsService _walletOperationsService;

    public ObservableCollection<WalletDto> Wallets { get; } = [];

    [ObservableProperty]
    private WalletDto? selectedWallet;

    [ObservableProperty]
    private string amountText = string.Empty;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    [ObservableProperty]
    private string successMessage = string.Empty;

    public bool IsErrorVisible => !string.IsNullOrWhiteSpace(ErrorMessage);

    public bool IsSuccessVisible => !string.IsNullOrWhiteSpace(SuccessMessage);

    public WithdrawViewModel(
        IUserService userService,
        IWalletOperationsService walletOperationsService)
    {
        _userService = userService;
        _walletOperationsService = walletOperationsService;
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;

            Wallets.Clear();

            var wallets = await _userService.GetWalletsAsync();

            foreach (var wallet in wallets)
                Wallets.Add(wallet);

            if (SelectedWallet is null && Wallets.Count > 0)
                SelectedWallet = Wallets[0];
        }
        catch (Exception ex)
        {
            ErrorMessage = GetErrorMessage(ex);
        }
        finally
        {
            IsBusy = false;
            NotifyStateChanged();
        }
    }

    [RelayCommand]
    private async Task WithdrawAsync()
    {
        if (IsBusy) return;

        ErrorMessage = string.Empty;
        SuccessMessage = string.Empty;

        if (SelectedWallet is null)
        {
            ErrorMessage = "Select a wallet.";
            NotifyStateChanged();
            return;
        }

        if (!decimal.TryParse(AmountText, out var amount) || amount <= 0)
        {
            ErrorMessage = "Enter a valid amount.";
            NotifyStateChanged();
            return;
        }

        try
        {
            IsBusy = true;
            NotifyStateChanged();

            var response = await _walletOperationsService.WithdrawAsync(SelectedWallet.WalletId, amount);
            var wallet = Wallets.FirstOrDefault(x => x.WalletId == response.WalletId);

            if (wallet is not null)
            {
                var index = Wallets.IndexOf(wallet);
                Wallets[index] = wallet with { Balance = response.Balance };
                SelectedWallet = Wallets[index];
            }

            AmountText = string.Empty;
            SuccessMessage = $"Withdrawn {amount:N2} {SelectedWallet.Currency}.";
        }
        catch (Exception ex)
        {
            ErrorMessage = GetErrorMessage(ex);
        }
        finally
        {
            IsBusy = false;
            NotifyStateChanged();
        }
    }

    private void NotifyStateChanged()
    {
        OnPropertyChanged(nameof(IsErrorVisible));
        OnPropertyChanged(nameof(IsSuccessVisible));
    }

    private static string GetErrorMessage(Exception exception)
    {
        return exception switch
        {
            ApiException { StatusCode: HttpStatusCode.UnprocessableEntity } => "Please enter a valid amount.",
            ApiException { StatusCode: HttpStatusCode.Conflict } => "The withdrawal could not be completed.",
            HttpRequestException => "Unable to connect to the server.",
            _ => "Unable to complete the withdrawal."
        };
    }
}