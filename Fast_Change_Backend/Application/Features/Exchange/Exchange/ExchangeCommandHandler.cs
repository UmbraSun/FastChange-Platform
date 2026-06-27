using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Service;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Resources;

namespace Application.Features.Exchange.Exchange;

public sealed class ExchangeCommandHandler
: IRequestHandler<ExchangeCommand, ExchangeResponse>
{
    private readonly IWalletRepository _walletRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IExchangeRateProvider _exchangeRateProvider;
    private readonly ICurrentUserService _currentUserService;
    private readonly IWalletOperationService _walletOperationService;
    private readonly IUnitOfWork _unitOfWork;

    public ExchangeCommandHandler(
        IWalletRepository walletRepository,
        ITransactionRepository transactionRepository,
        IExchangeRateProvider rateProvider,
        ICurrentUserService currentUser,
        IWalletOperationService walletOperationService,
        IUnitOfWork unitOfWork)
    {
        _walletRepository = walletRepository;
        _transactionRepository = transactionRepository;
        _exchangeRateProvider = rateProvider;
        _currentUserService = currentUser;
        _walletOperationService = walletOperationService;
        _unitOfWork = unitOfWork;
    }

    public async Task<ExchangeResponse> Handle(
    ExchangeCommand request,
    CancellationToken cancellationToken)
    {
        ExchangeResponse? response = null;

        await _unitOfWork.ExecuteAsync(async ct =>
        {
            var fromWallet = await _walletRepository.GetByIdAsync(request.FromWalletId, ct);
            var toWallet = await _walletRepository.GetByIdAsync(request.ToWalletId, ct);

            if (fromWallet is null || toWallet is null)
                throw new BusinessException(Localization.WalletNotFound);

            if (fromWallet.UserId != _currentUserService.UserId ||
                toWallet.UserId != _currentUserService.UserId)
                throw new BusinessException(Localization.WalletIsNotAssociatedWithThisUser);

            if (fromWallet.Balance < request.Amount)
                throw new BusinessException(Localization.InsufficientFunds);

            var rate = await _exchangeRateProvider.GetRateAsync(
                fromWallet.Currency,
                toWallet.Currency,
                ct);

            var received = request.Amount * rate.Rate;

            var fromResult = _walletOperationService.Withdraw(fromWallet, request.Amount);

            var toResult = _walletOperationService.Deposit(toWallet, received);

            await _walletRepository.UpdateAsync(fromWallet, ct);
            await _walletRepository.UpdateAsync(toWallet, ct);

            await _transactionRepository.AddAsync(fromResult.transaction, ct);
            await _transactionRepository.AddAsync(toResult.transaction, ct);

            response = new ExchangeResponse(
                request.Amount,
                received,
                rate.Rate);
        }, cancellationToken);

        return response!;
    }
}
