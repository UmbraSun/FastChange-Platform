using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.DTOs.Transfers;
using Core.DTOs.Wallets;
using Core.Interfaces;
using Refit;
using System.Collections.ObjectModel;
using System.Net;

namespace UI.ViewModels;

public partial class TransferViewModel : ObservableObject
{
    private readonly IUserService _userService;
    private readonly ITransferService _transferService;

    public ObservableCollection<WalletDto> Wallets { get; } = [];

    public ObservableCollection<TransferRecipientDto> Recipients { get; } = [];

    [ObservableProperty]
    private WalletDto? selectedWallet;

    [ObservableProperty]
    private TransferRecipientDto? selectedRecipient;

    [ObservableProperty]
    private string searchQuery = string.Empty;

    [ObservableProperty]
    private string amountText = string.Empty;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private bool isSearching;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    [ObservableProperty]
    private string successMessage = string.Empty;

    public bool IsErrorVisible => !string.IsNullOrWhiteSpace(ErrorMessage);

    public bool IsSuccessVisible => !string.IsNullOrWhiteSpace(SuccessMessage);

    public bool HasRecipients => Recipients.Count > 0;

    public bool HasSelectedRecipient => SelectedRecipient is not null;

    public TransferViewModel(
        IUserService userService,
        ITransferService transferService)
    {
        _userService = userService;
        _transferService = transferService;
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
            Recipients.Clear();

            SelectedRecipient = null;
            SearchQuery = string.Empty;
            AmountText = string.Empty;

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
    private async Task SearchRecipientsAsync()
    {
        if (IsBusy || IsSearching) return;

        Recipients.Clear();
        SelectedRecipient = null;
        NotifyStateChanged();

        if (SelectedWallet is null || string.IsNullOrWhiteSpace(SearchQuery) || SearchQuery.Trim().Length < 2)
            return;

        try
        {
            IsSearching = true;

            var recipients = await _transferService.SearchRecipientsAsync(SearchQuery.Trim(), SelectedWallet.Currency);

            foreach (var recipient in recipients)
                Recipients.Add(recipient);
        }
        catch (Exception ex)
        {
            ErrorMessage = GetErrorMessage(ex);
        }
        finally
        {
            IsSearching = false;
            NotifyStateChanged();
        }
    }

    public void SelectRecipient(TransferRecipientDto recipient)
    {
        SelectedRecipient = recipient;
        Recipients.Clear();
        NotifyStateChanged();
    }

    [RelayCommand]
    private async Task TransferAsync()
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

        if (SelectedRecipient is null)
        {
            ErrorMessage = "Select a recipient.";
            NotifyStateChanged();
            return;
        }

        if (SelectedWallet.Currency != SelectedRecipient.Currency)
        {
            ErrorMessage = "Recipient currency does not match the selected wallet.";
            NotifyStateChanged();
            return;
        }

        if (!decimal.TryParse(AmountText, out var amount) || amount <= 0)
        {
            ErrorMessage = "Enter a valid amount.";
            NotifyStateChanged();
            return;
        }

        if (amount > SelectedWallet.Balance)
        {
            ErrorMessage = "Insufficient balance.";
            NotifyStateChanged();
            return;
        }

        try
        {
            IsBusy = true;
            NotifyStateChanged();

            var response = await _transferService.TransferAsync(SelectedWallet.WalletId, SelectedRecipient.WalletId, amount);
            var wallet = Wallets.FirstOrDefault(x => x.WalletId == SelectedWallet.WalletId);

            if (wallet is not null)
            {
                var index = Wallets.IndexOf(wallet);
                Wallets[index] = wallet with { Balance = response.SenderBalance };
                SelectedWallet = Wallets[index];
            }

            AmountText = string.Empty;
            SuccessMessage = $"Transferred {response.Amount:N2} {SelectedWallet.Currency} to {SelectedRecipient.Email}.";

            SelectedRecipient = null;
            SearchQuery = string.Empty;
            Recipients.Clear();

            NotifyStateChanged();
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
        OnPropertyChanged(nameof(HasRecipients));
        OnPropertyChanged(nameof(HasSelectedRecipient));
    }

    private static string GetErrorMessage(Exception exception)
    {
        return exception switch
        {
            ApiException { StatusCode: HttpStatusCode.UnprocessableEntity } => "Please check the entered transfer data.",
            ApiException { StatusCode: HttpStatusCode.Conflict } => "The transfer could not be completed.",
            HttpRequestException => "Unable to connect to the server.",
            _ => "Unable to complete the transfer."
        };
    }
}