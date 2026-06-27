using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
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
    private readonly IUnitOfWork _unitOfWork;

    public ExchangeCommandHandler(
        IWalletRepository walletRepository,
        ITransactionRepository transactionRepository,
        IExchangeRateProvider rateProvider,
        ICurrentUserService currentUser,
        IUnitOfWork unitOfWork)
    {
        _walletRepository = walletRepository;
        _transactionRepository = transactionRepository;
        _exchangeRateProvider = rateProvider;
        _currentUserService = currentUser;
        _unitOfWork = unitOfWork;
    }

    public async Task<ExchangeResponse> Handle(
    ExchangeCommand request,
    CancellationToken cancellationToken)
    {
        ExchangeResponse? response = null;

        await _unitOfWork.ExecuteAsync(async ct =>
        {
            var fromWallet = await _walletRepository.GetByIdAsync(
                request.FromWalletId,
                ct);

            var toWallet = await _walletRepository.GetByIdAsync(
                request.ToWalletId,
                ct);

            if (fromWallet is null || toWallet is null)
                throw new BusinessException(Localization.WalletNotFound);

            if (fromWallet.UserId != _currentUserService.UserId ||
                toWallet.UserId != _currentUserService.UserId)
                throw new BusinessException(Localization.WalletIsNotAssociatedWithThisUser);

            var rate = await _exchangeRateProvider.GetRateAsync(
                fromWallet.Currency,
                toWallet.Currency,
                ct);

            var receivedAmount = request.Amount * rate.Rate;

            fromWallet.Withdraw(request.Amount);
            toWallet.Deposit(receivedAmount);

            await _walletRepository.UpdateAsync(fromWallet, ct);
            await _walletRepository.UpdateAsync(toWallet, ct);

            await _transactionRepository.AddAsync(
                Transaction.CreateExchange(
                    fromWallet,
                    toWallet,
                    request.Amount,
                    receivedAmount,
                    rate.Rate),
                ct);

            response = new ExchangeResponse(
                request.Amount,
                receivedAmount,
                rate.Rate);
        }, cancellationToken);

        return response!;
    }
}
