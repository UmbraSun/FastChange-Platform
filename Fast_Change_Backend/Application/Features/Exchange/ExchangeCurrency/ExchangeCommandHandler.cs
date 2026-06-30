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
    private readonly IWalletOperationService _walletOperationService;
    private readonly IExchangeRateProvider _exchangeRateProvider;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ExchangeCommandHandler(
        IWalletRepository walletRepository,
        IWalletAccessService walletAccessService,
        IWalletOperationService walletOperationService,
        IExchangeRateProvider exchangeRateProvider,
        ITransactionRepository transactionRepository,
        IUnitOfWork unitOfWork)
    {
        _walletRepository = walletRepository;
        _walletAccessService = walletAccessService;
        _walletOperationService = walletOperationService;
        _exchangeRateProvider = exchangeRateProvider;
        _transactionRepository = transactionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ExchangeResponse> Handle(
        ExchangeCommand request,
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

        await _walletAccessService.EnsureAccessAsync(
            fromWallet,
            cancellationToken);

        await _walletAccessService.EnsureAccessAsync(
            toWallet,
            cancellationToken);

        var rate = await _exchangeRateProvider.GetRateAsync(
            fromWallet.Currency,
            toWallet.Currency,
            cancellationToken);

        if (rate is null)
            throw new BusinessException(Localization.ExchangeRateNotFound);

        var result = _walletOperationService.Exchange(
            fromWallet,
            toWallet,
            request.Amount,
            rate.Rate);

        await _transactionRepository.AddRangeAsync(
        [
            result.withdrawTransaction,
            result.depositTransaction
        ],
        cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new ExchangeResponse(
            rate.Rate,
            request.Amount,
            result.receivedAmount,
            fromWallet.Balance,
            toWallet.Balance);
    }
}
