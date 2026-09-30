using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.DTOs.Wallets;
using Core.Interfaces;

namespace UI.ViewModels;

public partial class HistoryViewModel : ObservableObject
{
    private const int HistoryTakePerWallet = 50;

    private readonly IUserService _userService;
    private readonly ITransactionService _transactionService;

    private CancellationTokenSource? _loadCts;

    public ObservableCollection<WalletDto> Wallets { get; } = [];

    public ObservableCollection<HistoryWalletFilterViewModel> WalletFilters { get; } = [];

    public ObservableCollection<HistoryGroupViewModel> HistoryGroups { get; } = [];

    public IReadOnlyList<string> OperationTypes { get; } = [ "All types", "Deposit", "Transfer", "Withdraw" ];

    [ObservableProperty]
    private HistoryWalletFilterViewModel? selectedWalletFilter;

    [ObservableProperty]
    private string selectedOperationType = "All types";

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    public int OperationCount => HistoryGroups.Sum(x => x.Count);

    public bool HasWallets => Wallets.Count > 0;

    public bool HasHistory => HistoryGroups.Count > 0;

    public bool IsErrorVisible => !string.IsNullOrWhiteSpace(ErrorMessage);

    public bool IsEmptyWalletsVisible => !IsBusy && !IsErrorVisible && !HasWallets;

    public bool IsEmptyHistoryVisible => !IsBusy && !IsErrorVisible && HasWallets && !HasHistory;

    public bool IsContentVisible => !IsBusy && !IsErrorVisible && HasHistory;

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
        if (IsBusy) return;

        var cts = new CancellationTokenSource();
        _loadCts = cts;

        try
        {
            IsBusy = true;
            ErrorMessage = string.Empty;
            NotifyStateChanged();

            var wallets = await _userService.GetWalletsAsync(cts.Token);
            Wallets.Clear();
            foreach (var wallet in wallets)
                Wallets.Add(wallet);

            BuildWalletFilters();

            if (Wallets.Count == 0)
            {
                SelectedWalletFilter = null;
                HistoryGroups.Clear();
                NotifyStateChanged();
                NotifyHistoryStateChanged();

                return;
            }

            await LoadHistoryAsync(cts.Token);
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception)
        {
            ErrorMessage = "Unable to load history.";
            NotifyStateChanged();
            NotifyHistoryStateChanged();
        }
        finally
        {
            if (ReferenceEquals(_loadCts, cts))
                _loadCts = null;

            IsBusy = false;
            NotifyStateChanged();
            NotifyHistoryStateChanged();
            cts.Dispose();
        }
    }

    partial void OnSelectedWalletFilterChanged(HistoryWalletFilterViewModel? value)
    {
        if (IsBusy) return;

        _ = ReloadHistoryAsync();
    }

    partial void OnSelectedOperationTypeChanged(string value)
    {
        if (IsBusy) return;

        _ = ReloadHistoryAsync();
    }

    private void BuildWalletFilters()
    {
        WalletFilters.Clear();
        WalletFilters.Add(new HistoryWalletFilterViewModel(null, "All wallets"));

        foreach (var wallet in Wallets)
            WalletFilters.Add(new HistoryWalletFilterViewModel(wallet.WalletId, wallet.Currency));

        SelectedWalletFilter = WalletFilters[0];
    }

    private async Task ReloadHistoryAsync()
    {
        if (_loadCts is not null)
            return;

        var cts = new CancellationTokenSource();
        _loadCts = cts;

        try
        {
            IsBusy = true;
            ErrorMessage = string.Empty;

            NotifyStateChanged();
            await LoadHistoryAsync(cts.Token);
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception)
        {
            ErrorMessage = "Unable to load history.";
            NotifyStateChanged();
            NotifyHistoryStateChanged();
        }
        finally
        {
            if (ReferenceEquals(_loadCts, cts))
                _loadCts = null;

            IsBusy = false;
            NotifyStateChanged();
            NotifyHistoryStateChanged();
            cts.Dispose();
        }
    }

    private async Task LoadHistoryAsync(CancellationToken cancellationToken)
    {
        HistoryGroups.Clear();

        var walletsToLoad = SelectedWalletFilter?.WalletId is Guid walletId 
            ? Wallets.Where(x => x.WalletId == walletId).ToList()
            : Wallets.ToList();

        if (walletsToLoad.Count == 0)
        {
            NotifyHistoryStateChanged();
            return;
        }

        var historyTasks = walletsToLoad
            .Select(wallet => _transactionService.GetHistoryAsync(wallet.WalletId, HistoryTakePerWallet, cancellationToken))
            .ToArray();

        var results = await Task.WhenAll(historyTasks);
        cancellationToken.ThrowIfCancellationRequested();
        var walletCurrencies = walletsToLoad.ToDictionary(wallet => wallet.WalletId, wallet => wallet.Currency);

        var items = results.SelectMany(result => result)
            .Where(item => MatchesOperationType(item.OperationType))
            .OrderByDescending(item => item.CreatedAtUtc)
            .Select(item =>
            {
                walletCurrencies.TryGetValue(item.WalletId, out var currency);
                return new HistoryItemViewModel(item, currency ?? string.Empty);
            })
            .ToList();

        var groups = items.GroupBy(item => item.CreatedAtUtc.ToLocalTime().Date)
            .OrderByDescending(group => group.Key);

        foreach (var group in groups)
            HistoryGroups.Add(new HistoryGroupViewModel(group.Key, group));

        NotifyHistoryStateChanged();
    }

    private bool MatchesOperationType(string operationType)
    {
        if (string.Equals(SelectedOperationType, "All types", StringComparison.OrdinalIgnoreCase))
            return true;

        return string.Equals(operationType, SelectedOperationType, StringComparison.OrdinalIgnoreCase);
    }

    private void NotifyStateChanged()
    {
        OnPropertyChanged(nameof(IsErrorVisible));
        OnPropertyChanged(nameof(IsEmptyWalletsVisible));
        OnPropertyChanged(nameof(IsEmptyHistoryVisible));
        OnPropertyChanged(nameof(IsContentVisible));
    }

    private void NotifyHistoryStateChanged()
    {
        OnPropertyChanged(nameof(HasHistory));
        OnPropertyChanged(nameof(OperationCount));
        OnPropertyChanged(nameof(IsEmptyHistoryVisible));
        OnPropertyChanged(nameof(IsContentVisible));
    }
}