using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.DTOs.Transactions;
using Core.DTOs.Wallets;
using Core.Interfaces;

namespace UI.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    private const int RecentActivityCount = 5;
    private const int RecentActivityPageSize = 5;

    private static readonly string[] MarketCurrencies = ["BTC", "ETH", "SOL"];
    
    private static readonly TimeSpan HomeCacheDuration = TimeSpan.FromSeconds(20);

    private readonly IUserService _userService;
    private readonly IPortfolioService _portfolioService;
    private readonly ITransactionService _transactionService;

    private CancellationTokenSource? _loadCts;
    private DateTimeOffset _lastSuccessfulLoad = DateTimeOffset.MinValue;

    public ObservableCollection<WalletDto> Wallets { get; } = [];

    public ObservableCollection<MarketItemViewModel> Markets { get; } = [];

    public ObservableCollection<RecentActivityItemViewModel> RecentActivity { get; } = [];

    [ObservableProperty]
    private string userName = string.Empty;

    [ObservableProperty]
    private decimal totalBalance;

    [ObservableProperty]
    private decimal balanceChange;

    [ObservableProperty]
    private decimal balanceChangePercent;

    [ObservableProperty]
    private bool isBalanceVisible = true;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    [ObservableProperty]
    private string recentActivityErrorMessage = string.Empty;

    public string DisplayBalance => IsBalanceVisible ? TotalBalance.ToString("N2") : "••••••";

    public string DisplayBalanceChange => IsBalanceVisible ? $"{BalanceChange:+0.00;-0.00;0.00}" : "••••••";

    public string DisplayBalanceChangePercent => IsBalanceVisible ? $"{BalanceChangePercent:+0.00;-0.00;0.00}%" : "••••••";

    public bool HasPositiveBalanceChange => BalanceChange > 0;

    public bool HasNegativeBalanceChange => BalanceChange < 0;

    public bool HasNoBalanceChange => BalanceChange == 0;

    public bool HasWallets => Wallets.Count > 0;

    public bool HasMarkets => Markets.Count > 0;

    public bool HasRecentActivity => RecentActivity.Count > 0;

    public bool IsMarketsEmpty => !IsBusy && !IsErrorVisible && !HasMarkets;

    public bool IsRecentActivityErrorVisible => !string.IsNullOrWhiteSpace(RecentActivityErrorMessage);

    public bool IsRecentActivityEmpty => !IsBusy && !IsRecentActivityErrorVisible && !HasRecentActivity;

    public bool IsErrorVisible => !string.IsNullOrWhiteSpace(ErrorMessage);

    public bool IsEmptyVisible => !IsBusy && !IsErrorVisible && !HasWallets;

    public bool IsContentVisible => !IsBusy && !IsErrorVisible && HasWallets;

    public HomeViewModel(
        IUserService userService,
        IPortfolioService portfolioService,
        ITransactionService transactionService)
    {
        _userService = userService;
        _portfolioService = portfolioService;
        _transactionService = transactionService;
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        await LoadInternalAsync(forceRefresh: false);
    }

    public async Task RefreshAsync()
    {
        await LoadInternalAsync(forceRefresh: true);
    }

    private async Task LoadInternalAsync(bool forceRefresh)
    {
        if (IsBusy) return;

        if (!forceRefresh && DateTimeOffset.UtcNow - _lastSuccessfulLoad < HomeCacheDuration)
            return;

        var cts = new CancellationTokenSource();
        _loadCts = cts;

        try
        {
            IsBusy = true;
            ErrorMessage = string.Empty;
            RecentActivityErrorMessage = string.Empty;

            NotifyStateChanged();
            NotifyRecentActivityStateChanged();

            var userTask = _userService.GetCurrentUserAsync(cts.Token);
            var walletsTask = _userService.GetWalletsAsync(cts.Token);
            var portfolioTask = _portfolioService.GetPortfolioAsync("USD", cts.Token);
            var performanceTask = _portfolioService.GetPerformanceAsync("USD", cts.Token);
            var marketTask = _portfolioService.GetMarketOverviewAsync(MarketCurrencies, "USD", cts.Token);

            await Task.WhenAll(userTask, walletsTask, portfolioTask, performanceTask, marketTask);

            cts.Token.ThrowIfCancellationRequested();

            var user = await userTask;
            var wallets = await walletsTask;
            var portfolio = await portfolioTask;
            var performance = await performanceTask;
            var markets = await marketTask;

            UserName = user.Email;

            Wallets.Clear();
            foreach (var wallet in wallets)
                Wallets.Add(wallet);

            TotalBalance = portfolio.TotalBalance;
            BalanceChange = performance.ChangeAmount;
            BalanceChangePercent = performance.ChangePercent;

            Markets.Clear();
            foreach (var market in markets)
                Markets.Add(new MarketItemViewModel(market));

            await LoadRecentActivityAsync(wallets, cts.Token);
            cts.Token.ThrowIfCancellationRequested();
            _lastSuccessfulLoad = DateTimeOffset.UtcNow;

            NotifyStateChanged();
            NotifyBalanceStateChanged();
            NotifyRecentActivityStateChanged();
        }
        catch (OperationCanceledException)
        {
            // The current load was cancelled.
        }
        catch (Exception)
        {
            ErrorMessage = "Unable to load account data.";
            NotifyStateChanged();
        }
        finally
        {
            if (ReferenceEquals(_loadCts, cts))
                _loadCts = null;

            IsBusy = false;
            NotifyStateChanged();
            NotifyRecentActivityStateChanged();
            cts.Dispose();
        }
    }

    private async Task LoadRecentActivityAsync(
        IReadOnlyList<WalletDto> wallets,
        CancellationToken cancellationToken)
    {
        RecentActivity.Clear();
        RecentActivityErrorMessage = string.Empty;

        if (wallets.Count == 0)
        {
            NotifyRecentActivityStateChanged();
            return;
        }

        try
        {
            var historyTasks = wallets.Select(wallet =>
                _transactionService.GetHistoryAsync(wallet.WalletId, RecentActivityPageSize, cancellationToken))
                .ToArray();

            var results = await Task.WhenAll(historyTasks);
            cancellationToken.ThrowIfCancellationRequested();
            var recentItems = results
                .SelectMany(result => result)
                .OrderByDescending(item => item.CreatedAtUtc)
                .Take(RecentActivityCount)
                .ToList();

            var walletCurrencies = wallets.ToDictionary(wallet => wallet.WalletId, wallet => wallet.Currency);

            foreach (var item in recentItems)
            {
                if (!walletCurrencies.TryGetValue(item.WalletId, out var currency))
                    continue;

                RecentActivity.Add(new RecentActivityItemViewModel(item, currency));
            }
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception)
        {
            RecentActivity.Clear();
            RecentActivityErrorMessage = "Unable to load recent activity.";
        }

        NotifyRecentActivityStateChanged();
    }

    [RelayCommand]
    private void ToggleBalanceVisibility()
    {
        IsBalanceVisible = !IsBalanceVisible;

        NotifyBalanceStateChanged();
    }

    private void NotifyBalanceStateChanged()
    {
        OnPropertyChanged(nameof(DisplayBalance));
        OnPropertyChanged(nameof(DisplayBalanceChange));
        OnPropertyChanged(nameof(DisplayBalanceChangePercent));
        OnPropertyChanged(nameof(HasPositiveBalanceChange));
        OnPropertyChanged(nameof(HasNegativeBalanceChange));
        OnPropertyChanged(nameof(HasNoBalanceChange));
    }

    private void NotifyStateChanged()
    {
        OnPropertyChanged(nameof(HasWallets));
        OnPropertyChanged(nameof(HasMarkets));
        OnPropertyChanged(nameof(IsMarketsEmpty));
        OnPropertyChanged(nameof(IsErrorVisible));
        OnPropertyChanged(nameof(IsEmptyVisible));
        OnPropertyChanged(nameof(IsContentVisible));
    }

    private void NotifyRecentActivityStateChanged()
    {
        OnPropertyChanged(nameof(HasRecentActivity));
        OnPropertyChanged(nameof(IsRecentActivityErrorVisible));
        OnPropertyChanged(nameof(IsRecentActivityEmpty));
    }
}