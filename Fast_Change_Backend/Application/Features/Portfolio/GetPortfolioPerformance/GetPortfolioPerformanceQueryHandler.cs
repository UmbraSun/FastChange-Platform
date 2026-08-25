using Application.Common.Interfaces;
using Contracts.Enums;
using MediatR;

namespace Application.Features.Portfolio.GetPortfolioPerformance;

public sealed class GetPortfolioPerformanceQueryHandler
    : IRequestHandler<GetPortfolioPerformanceQuery, PortfolioPerformanceResponse>
{
    private readonly IWalletAccessService _walletAccessService;
    private readonly IWalletHistoryReader _walletHistoryReader;
    private readonly IExchangeRateProvider _exchangeRateProvider;
    private readonly IHistoricalExchangeRateProvider _historicalExchangeRateProvider;

    public GetPortfolioPerformanceQueryHandler(
        IWalletAccessService walletAccessService,
        IWalletHistoryReader walletHistoryReader,
        IExchangeRateProvider exchangeRateProvider,
        IHistoricalExchangeRateProvider historicalExchangeRateProvider)
    {
        _walletAccessService = walletAccessService;
        _walletHistoryReader = walletHistoryReader;
        _exchangeRateProvider = exchangeRateProvider;
        _historicalExchangeRateProvider = historicalExchangeRateProvider;
    }

    public async Task<PortfolioPerformanceResponse> Handle(
        GetPortfolioPerformanceQuery request,
        CancellationToken cancellationToken)
    {
        var targetCurrency = request.Currency.ToUpperInvariant();
        var wallets = await _walletAccessService.GetOwnedWalletsAsync(cancellationToken);

        if (wallets.Count == 0)
            return new PortfolioPerformanceResponse(targetCurrency, 0m, 0m, 0m, 0m);

        var startOfDayUtc = DateTime.UtcNow.Date;
        var currentValue = 0m;
        var previousValue = 0m;
        var netExternalCashFlow = 0m;

        foreach (var wallet in wallets)
        {
            var walletCurrency = wallet.Currency.ToUpperInvariant();
            var history = await _walletHistoryReader.GetByWalletAsync(wallet.Id, 10_000, cancellationToken);
            var todayHistory = history.Where(x => x.CreatedAtUtc >= startOfDayUtc).ToList();
            var todaySignedAmount = todayHistory.Sum(x => x.SignedAmount);
            var previousBalance = wallet.Balance - todaySignedAmount;
            var currentRate = await GetRateAsync(walletCurrency, targetCurrency, cancellationToken);
            var historicalRate = await GetHistoricalRateAsync(walletCurrency, targetCurrency, DateOnly.FromDateTime(startOfDayUtc), cancellationToken);

            currentValue += wallet.Balance * currentRate;
            previousValue += previousBalance * historicalRate;

            netExternalCashFlow += todayHistory
                .Where(x => 
                    x.OperationType == nameof(TransactionType.Deposit) ||
                    x.OperationType == nameof(TransactionType.Withdraw))
                .Sum(x => x.SignedAmount * currentRate);
        }

        var changeAmount = currentValue - previousValue - netExternalCashFlow;
        var changePercent = previousValue == 0m ? 0m : changeAmount / previousValue * 100m;

        return new PortfolioPerformanceResponse(targetCurrency,
            decimal.Round(currentValue, 8),
            decimal.Round(previousValue, 8),
            decimal.Round(changeAmount, 8),
            decimal.Round(changePercent, 2));
    }

    private async Task<decimal> GetRateAsync(
        string fromCurrency,
        string toCurrency,
        CancellationToken cancellationToken)
    {
        if (string.Equals(fromCurrency, toCurrency, StringComparison.OrdinalIgnoreCase))
            return 1m;

        var result = await _exchangeRateProvider.GetRateAsync(fromCurrency, toCurrency, cancellationToken);
        return result.Rate;
    }

    private async Task<decimal> GetHistoricalRateAsync(
        string fromCurrency,
        string toCurrency,
        DateOnly date,
        CancellationToken cancellationToken)
    {
        if (string.Equals(fromCurrency, toCurrency, StringComparison.OrdinalIgnoreCase))
            return 1m;

        var result = await _historicalExchangeRateProvider.GetRateAsync(fromCurrency, toCurrency, date, cancellationToken);
        return result.Rate;
    }
}
