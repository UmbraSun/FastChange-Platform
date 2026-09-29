using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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

            var userTask = LoadUserAsync(cts.Token);
            var walletsTask = LoadWalletsAsync(cts.Token);
            
            await Task.WhenAll(userTask, walletsTask);

            cts.Token.ThrowIfCancellationRequested();
            
            var wallets = await walletsTask;
            var portfolioTask = LoadPortfolioAsync(cts.Token);
            var performanceTask = LoadPerformanceAsync(cts.Token);
            var marketsTask = LoadMarketsAsync(cts.Token);
            var recentActivityTask = LoadRecentActivityAsync(wallets, cts.Token);

            await Task.WhenAll(portfolioTask, performanceTask, marketsTask, recentActivityTask);

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

    private async Task LoadUserAsync(CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userService.GetCurrentUserAsync(cancellationToken);
            UserName = user.Email;
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception)
        {
            ErrorMessage = "Unable to load account data.";
        }
    }

    private async Task<IReadOnlyList<WalletDto>> LoadWalletsAsync(CancellationToken cancellationToken)
    {
        try
        {
            var wallets = await _userService.GetWalletsAsync(cancellationToken);
            Wallets.Clear();
            foreach (var wallet in wallets)
                Wallets.Add(wallet);

            OnPropertyChanged(nameof(HasWallets));
            return wallets;
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception)
        {
            ErrorMessage = "Unable to load account data.";
            Wallets.Clear();
            OnPropertyChanged(nameof(HasWallets));
            return [];
        }
    }

    private async Task LoadPortfolioAsync(CancellationToken cancellationToken)
    {
        try
        {
            var portfolio = await _portfolioService.GetPortfolioAsync("USD", cancellationToken);
            TotalBalance = portfolio.TotalBalance;
            OnPropertyChanged(nameof(DisplayBalance));
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception)
        {
        }
    }

    private async Task LoadPerformanceAsync(CancellationToken cancellationToken)
    {
        try
        {
            var performance = await _portfolioService.GetPerformanceAsync("USD", cancellationToken);
            BalanceChange = performance.ChangeAmount;
            BalanceChangePercent = performance.ChangePercent;

            NotifyBalanceStateChanged();
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception)
        {
        }
    }

    private async Task LoadMarketsAsync(CancellationToken cancellationToken)
    {
        try
        {
            var markets = await _portfolioService.GetMarketOverviewAsync(MarketCurrencies, "USD", cancellationToken);

            Markets.Clear();
            foreach (var market in markets)
                Markets.Add(new MarketItemViewModel(market));
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception)
        {
            Markets.Clear();
        }

        OnPropertyChanged(nameof(HasMarkets));
        OnPropertyChanged(nameof(IsMarketsEmpty));
    }

    private async Task LoadRecentActivityAsync(IReadOnlyList<WalletDto> wallets, CancellationToken cancellationToken)
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
            var walletCurrencies = wallets.ToDictionary(wallet => wallet.WalletId, wallet => wallet.Currency);
            var recentItems = results
                .SelectMany(result => result)
                .OrderByDescending(item => item.CreatedAtUtc)
                .Take(RecentActivityCount)
                .ToList();

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