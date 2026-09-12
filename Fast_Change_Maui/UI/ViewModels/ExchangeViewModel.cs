using System.Collections.ObjectModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.DTOs.Wallets;
using Core.Interfaces;

namespace UI.ViewModels;

public partial class ExchangeViewModel : ObservableObject
{
    private readonly IUserService _userService;
    private readonly IExchangeService _exchangeService;

    public ObservableCollection<WalletDto> Wallets { get; } = [];

    [ObservableProperty]
    private WalletDto? fromWallet;

    [ObservableProperty]
    private WalletDto? toWallet;

    [ObservableProperty]
    private string amount = string.Empty;

    [ObservableProperty]
    private decimal? receivedAmount;

    [ObservableProperty]
    private decimal? exchangeRate;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private bool isPreviewLoading;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    [ObservableProperty]
    private string successMessage = string.Empty;

    public ExchangeViewModel(IUserService userService, IExchangeService exchangeService)
    {
        _userService = userService;
        _exchangeService = exchangeService;
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        if (IsBusy)
        {
            return;
        }

        try
        {
            IsBusy = true;
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;

            var wallets = await _userService.GetWalletsAsync();
            Wallets.Clear();

            foreach (var wallet in wallets)
                Wallets.Add(wallet);

            if (Wallets.Count >= 2)
            {
                FromWallet = Wallets[0];
                ToWallet = Wallets[1];
            }
            else
            {
                FromWallet = null;
                ToWallet = null;
            }

            ClearPreview();
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

    partial void OnFromWalletChanged(WalletDto? value)
    {
        if (value is null)
        {
            ClearPreview();
            return;
        }

        if (ToWallet?.WalletId == value.WalletId)
            ToWallet = Wallets.FirstOrDefault(wallet => wallet.WalletId != value.WalletId);

        ClearMessages();
        ClearPreview();
    }

    partial void OnToWalletChanged(WalletDto? value)
    {
        if (value is null)
        {
            ClearPreview();
            return;
        }

        if (FromWallet?.WalletId == value.WalletId)
            FromWallet = Wallets.FirstOrDefault(wallet => wallet.WalletId != value.WalletId);

        ClearMessages();
        ClearPreview();
    }

    [RelayCommand]
    private async Task PreviewAsync()
    {
        if (IsPreviewLoading) return;

        ErrorMessage = string.Empty;

        if (!TryGetAmount(out var parsedAmount))
        {
            ClearPreview();
            return;
        }

        if (FromWallet is null || ToWallet is null)
        {
            ClearPreview();
            return;
        }

        if (FromWallet.WalletId == ToWallet.WalletId)
        {
            ClearPreview();
            ErrorMessage = "Select different wallets.";
            return;
        }

        if (parsedAmount <= 0)
        {
            ClearPreview();
            return;
        }

        try
        {
            IsPreviewLoading = true;

            var response = await _exchangeService.PreviewAsync(FromWallet.WalletId, ToWallet.WalletId, parsedAmount);

            ExchangeRate = response.ExchangeRate;
            ReceivedAmount = response.ReceivedAmount;
        }
        catch (OperationCanceledException)
        {
            // Expected when a newer preview replaces this request.
        }
        catch (Exception)
        {
            ClearPreview();
            ErrorMessage = "Unable to calculate exchange.";
        }
        finally
        {
            IsPreviewLoading = false;
        }
    }

    [RelayCommand]
    private async Task ExchangeAsync()
    {
        if (IsBusy) return;

        ErrorMessage = string.Empty;
        SuccessMessage = string.Empty;

        if (!TryGetAmount(out var parsedAmount))
        {
            ErrorMessage = "Enter a valid amount.";
            return;
        }

        if (FromWallet is null || ToWallet is null)
        {
            ErrorMessage = "Select source and destination wallets.";
            return;
        }

        if (FromWallet.WalletId == ToWallet.WalletId)
        {
            ErrorMessage = "Select different wallets.";
            return;
        }

        if (parsedAmount <= 0)
        {
            ErrorMessage = "Amount must be greater than zero.";
            return;
        }

        try
        {
            IsBusy = true;

            var sourceWalletId = FromWallet.WalletId;
            var destinationWalletId = ToWallet.WalletId;

            var response = await _exchangeService.ExchangeAsync(sourceWalletId, destinationWalletId, parsedAmount);

            UpdateWalletBalance(sourceWalletId, response.SourceBalance);
            UpdateWalletBalance(destinationWalletId, response.DestinationBalance);

            ExchangeRate = response.ExchangeRate;
            ReceivedAmount = response.ReceivedAmount;
            SuccessMessage = $"Exchanged {response.SentAmount:N2} {FromWallet.Currency} to {response.ReceivedAmount:N2} {ToWallet.Currency}.";
        }
        catch (Exception)
        {
            ErrorMessage = "Exchange failed.";
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private void SwapWallets()
    {
        if (FromWallet is null || ToWallet is null) return;
        (FromWallet, ToWallet) = (ToWallet, FromWallet);

        ClearMessages();
        ClearPreview();
    }

    private bool TryGetAmount(out decimal value)
    {
        return decimal.TryParse(Amount, NumberStyles.Number, CultureInfo.InvariantCulture, out value);
    }

    private void UpdateWalletBalance(Guid walletId, decimal balance)
    {
        var index = Wallets.Select((wallet, index) => new { wallet, index })
            .FirstOrDefault(x => x.wallet.WalletId == walletId)?.index;

        if (index is null) return;

        var wallet = Wallets[index.Value];

        Wallets[index.Value] = wallet with { Balance = balance };

        if (FromWallet?.WalletId == walletId)
            FromWallet = Wallets[index.Value];

        if (ToWallet?.WalletId == walletId)
            ToWallet = Wallets[index.Value];
    }

    private void ClearPreview()
    {
        ReceivedAmount = null;
        ExchangeRate = null;
    }

    private void ClearMessages()
    {
        ErrorMessage = string.Empty;
        SuccessMessage = string.Empty;
    }
}