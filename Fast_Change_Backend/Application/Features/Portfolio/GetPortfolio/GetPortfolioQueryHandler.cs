using Application.Common.Interfaces;
using MediatR;

namespace Application.Features.Portfolio.GetPortfolio;

public sealed class GetPortfolioQueryHandler
    : IRequestHandler<GetPortfolioQuery, PortfolioResponse>
{
    private readonly IWalletAccessService _walletAccessService;
    private readonly IExchangeRateProvider _exchangeRateProvider;

    public GetPortfolioQueryHandler(
        IWalletAccessService walletAccessService,
        IExchangeRateProvider exchangeRateProvider)
    {
        _walletAccessService = walletAccessService;
        _exchangeRateProvider = exchangeRateProvider;
    }

    public async Task<PortfolioResponse> Handle(
        GetPortfolioQuery request,
        CancellationToken cancellationToken)
    {
        var wallets = await _walletAccessService.GetOwnedWalletsAsync(cancellationToken);
        var targetCurrency = request.Currency.ToUpperInvariant();
        var result = new List<PortfolioWalletResponse>();

        foreach (var wallet in wallets)
        {
            var walletCurrency = wallet.Currency.ToUpperInvariant();

            if (walletCurrency == targetCurrency)
            {
                result.Add(new PortfolioWalletResponse(
                    wallet.Id,
                    wallet.Currency,
                    wallet.Balance,
                    1m,
                    wallet.Balance));
                continue;
            }

            var exchangeRate = await _exchangeRateProvider.GetRateAsync(walletCurrency, targetCurrency, cancellationToken);

            var convertedValue = decimal.Round(
                wallet.Balance * exchangeRate.Rate,
                8,
                MidpointRounding.ToEven);

            result.Add(new PortfolioWalletResponse(
                wallet.Id,
                wallet.Currency,
                wallet.Balance,
                exchangeRate.Rate,
                convertedValue));
        }

        var totalBalance = result.Sum(x => x.ConvertedValue);
        return new PortfolioResponse(targetCurrency, totalBalance, result);
    }
}
