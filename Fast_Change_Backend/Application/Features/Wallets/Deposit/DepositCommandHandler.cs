using Application.Common.Interfaces;
using MediatR;

namespace Application.Features.Wallets.Deposit;

public class DepositCommandHandler 
    : IRequestHandler<DepositCommand, DepositResponse>
{
    private readonly IWalletRepository _walletRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IWalletOperationService _walletOperationService;
    private readonly IWalletAccessService _walletAccessService;
    private readonly IUnitOfWork _unitOfWork;

    public DepositCommandHandler(
        IWalletRepository walletRepository,
        ITransactionRepository transactionRepository,
        IWalletOperationService walletOperationService,
        IWalletAccessService walletAccessService,
        IUnitOfWork unitOfWork)
    {
        _walletRepository = walletRepository;
        _transactionRepository = transactionRepository;
        _walletOperationService = walletOperationService;
        _walletAccessService = walletAccessService;
        _unitOfWork = unitOfWork;
    }

    public async Task<DepositResponse> Handle(
        DepositCommand request,
        CancellationToken cancellationToken)
    {
        var wallet = await _walletAccessService.GetOwnedWalletAsync(
            request.WalletId,
            cancellationToken);

        var result = _walletOperationService.Deposit(wallet, request.Amount);

        await _unitOfWork.ExecuteAsync(async ct =>
        {
            await _walletRepository.UpdateAsync(wallet, ct);
            await _transactionRepository.AddAsync(result.transaction, ct);
        }, cancellationToken);

        return new DepositResponse(
            wallet.Id,
            wallet.Balance);
    }
}
