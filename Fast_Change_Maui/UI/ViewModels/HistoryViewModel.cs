using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.DTOs.Transactions;
using Core.DTOs.Wallets;
using Core.Interfaces;

namespace UI.ViewModels;

public partial class HistoryViewModel : ObservableObject
{
    private readonly IUserService _userService;
    private readonly ITransactionService _transactionService;

    public ObservableCollection<WalletDto> Wallets { get; } = [];

    public ObservableCollection<TransactionDto> Transactions { get; } = [];

    [ObservableProperty]
    private WalletDto? selectedWallet;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    public HistoryViewModel(
        IUserService userService,
        ITransactionService transactionService)
    {
        _userService = userService;
        _transactionService = transactionService;
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

            var wallets = await _userService.GetWalletsAsync();

            Wallets.Clear();

            foreach (var wallet in wallets)
                Wallets.Add(wallet);

            if (Wallets.Count == 0)
            {
                SelectedWallet = null;
                Transactions.Clear();
                return;
            }

            if (SelectedWallet is null || Wallets.All(x => x.WalletId != SelectedWallet.WalletId))
                SelectedWallet = Wallets[0];

            await LoadTransactionsAsync(SelectedWallet.WalletId);
        }
        catch (Exception)
        {
            ErrorMessage = "Unable to load transaction history.";
        }
        finally
        {
            IsBusy = false;
        }
    }

    partial void OnSelectedWalletChanged(WalletDto? value)
    {
        if (value is null || IsBusy) return;

        _ = LoadSelectedWalletAsync(value.WalletId);
    }

    private async Task LoadSelectedWalletAsync(Guid walletId)
    {
        try
        {
            IsBusy = true;
            ErrorMessage = string.Empty;

            await LoadTransactionsAsync(walletId);
        }
        catch (Exception)
        {
            ErrorMessage = "Unable to load transaction history.";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task LoadTransactionsAsync(Guid walletId)
    {
        var result = await _transactionService.GetTransactionsAsync(walletId);
        Transactions.Clear();

        foreach (var transaction in result.Items)
            Transactions.Add(transaction);
    }
}