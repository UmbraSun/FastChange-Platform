using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.DTOs.Transactions;
using Core.Interfaces;
using System.Collections.ObjectModel;

namespace UI.ViewModels;

public partial class HistoryViewModel : ObservableObject
{
    private readonly IUserService _userService;
    private readonly ITransactionService _transactionService;

    public ObservableCollection<TransactionDto> Transactions { get; } = [];

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    public HistoryViewModel(IUserService userService, ITransactionService transactionService)
    {
        _userService = userService;
        _transactionService = transactionService;
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

            if (wallets.Count == 0)
            {
                Transactions.Clear();
                return;
            }

            var result = await _transactionService.GetTransactionsAsync(wallets[0].WalletId);
            Transactions.Clear();

            foreach (var transaction in result.Items)
                Transactions.Add(transaction);
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
}