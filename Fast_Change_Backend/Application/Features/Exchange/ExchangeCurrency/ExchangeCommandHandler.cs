using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Resources;

namespace Application.Features.Exchange.ExchangeCurrency;

public sealed class ExchangeCommandHandler
    : IRequestHandler<ExchangeCommand, ExchangeResponse>
{
    private readonly IWalletRepository _walletRepository;
    private readonly IWalletAccessService _walletAccessService;
    private readonly IExchangeRateProvider _exchangeRateProvider;
    private readonly IExchangeService _exchangeService;
    private readonly IUnitOfWork _unitOfWork;

    public ExchangeCommandHandler(
        IWalletRepository walletRepository,
        IWalletAccessService walletAccessService,
        IExchangeRateProvider exchangeRateProvider,
        IExchangeService exchangeService,
        IUnitOfWork unitOfWork)
    {
        _walletRepository = walletRepository;
        _walletAccessService = walletAccessService;
        _exchangeRateProvider = exchangeRateProvider;
        _exchangeService = exchangeService;
        _unitOfWork = unitOfWork;
    }

    public async Task<ExchangeResponse> Handle(
        ExchangeCommand request,
        CancellationToken cancellationToken)
    {
        var fromWallet = await _walletRepository.GetByIdAsync(request.FromWalletId, cancellationToken)
            ?? throw new BusinessException(Localization.WalletNotFound);

        var toWallet = await _walletRepository.GetByIdAsync(request.ToWalletId, cancellationToken)
            ?? throw new BusinessException(Localization.WalletNotFound);

        await _walletAccessService.EnsureAccessAsync(fromWallet, cancellationToken);
        await _walletAccessService.EnsureAccessAsync(toWallet, cancellationToken);

        var rate = await _exchangeRateProvider.GetRateAsync(
            fromWallet.Currency,
            toWallet.Currency,
            cancellationToken)
            ?? throw new BusinessException(Localization.ExchangeRateNotFound);

        var result = _exchangeService.Exchange(
            fromWallet,
            toWallet,
            request.Amount,
            rate.Rate);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new ExchangeResponse(
            rate.Rate,
            request.Amount,
            result.receivedAmount,
            fromWallet.Balance,
            toWallet.Balance);
    }
}
