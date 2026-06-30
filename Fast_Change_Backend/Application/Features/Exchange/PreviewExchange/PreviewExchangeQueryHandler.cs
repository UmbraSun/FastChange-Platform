using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Resources;

namespace Application.Features.Exchange.PreviewExchange;

public sealed class PreviewExchangeQueryHandler
    : IRequestHandler<PreviewExchangeQuery, PreviewExchangeResponse>
{
    private readonly IWalletRepository _walletRepository;
    private readonly IExchangeRateProvider _exchangeRateProvider;

    public PreviewExchangeQueryHandler(
        IWalletRepository walletRepository,
        IExchangeRateProvider exchangeRateProvider)
    {
        _walletRepository = walletRepository;
        _exchangeRateProvider = exchangeRateProvider;
    }

    public async Task<PreviewExchangeResponse> Handle(
        PreviewExchangeQuery request,
        CancellationToken cancellationToken)
    {
        var fromWallet = await _walletRepository.GetByIdAsync(
            request.FromWalletId,
            cancellationToken)
            ?? throw new BusinessException(Localization.WalletNotFound);

        var toWallet = await _walletRepository.GetByIdAsync(
            request.ToWalletId,
            cancellationToken)
            ?? throw new BusinessException(Localization.WalletNotFound);

        var rate = await _exchangeRateProvider.GetRateAsync(
            fromWallet.Currency,
            toWallet.Currency,
            cancellationToken)
            ?? throw new BusinessException(Localization.ExchangeRateNotFound);

        var receivedAmount = decimal.Round(
            request.Amount * rate.Rate,
            8,
            MidpointRounding.ToEven);

        return new PreviewExchangeResponse(
            rate.Rate,
            request.Amount,
            receivedAmount);
    }
}
