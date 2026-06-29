using Application.Common.Interfaces;
using MediatR;

namespace Application.Features.Exchange.Exchange;

public sealed class ExchangeCommandHandler
    : IRequestHandler<ExchangeCommand, ExchangeResponse>
{
    private readonly IWalletRepository _walletRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IExchangeRateProvider _exchangeRateProvider;
    private readonly IWalletOperationService _walletOperationService;
    private readonly IWalletAccessService _walletAccessService;
    private readonly IUnitOfWork _unitOfWork;

    public ExchangeCommandHandler(
        IWalletRepository walletRepository,
        ITransactionRepository transactionRepository,
        IExchangeRateProvider rateProvider,
        IWalletOperationService walletOperationService,
        IWalletAccessService walletAccessService,
        IUnitOfWork unitOfWork)
    {
        _walletRepository = walletRepository;
        _transactionRepository = transactionRepository;
        _exchangeRateProvider = rateProvider;
        _walletOperationService = walletOperationService;
        _walletAccessService = walletAccessService;
        _unitOfWork = unitOfWork;
    }

    public async Task<ExchangeResponse> Handle(
        ExchangeCommand request,
        CancellationToken cancellationToken)
    {
        ExchangeResponse? response = null;

        await _unitOfWork.ExecuteAsync(async ct =>
        {
            var fromWallet = await _walletAccessService.GetOwnedWalletAsync(
                request.FromWalletId,
                cancellationToken);

            var toWallet = await _walletAccessService.GetOwnedWalletAsync(
                request.ToWalletId,
                cancellationToken);

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
