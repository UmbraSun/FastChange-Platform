using Application.Common.Interfaces;
using MediatR;

namespace Application.Features.Wallets.Withdraw;

public class WithdrawCommandHandler
    : IRequestHandler<WithdrawCommand, WithdrawResponse>
{
    private readonly IWalletRepository _walletRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IWalletOperationService _walletOperationService;
    private readonly IWalletAccessService _walletAccessService;
    private readonly IUnitOfWork _unitOfWork;

    public WithdrawCommandHandler(
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

    public async Task<WithdrawResponse> Handle(
        WithdrawCommand request,
        CancellationToken cancellationToken)
    {
        var wallet = await _walletAccessService.GetOwnedWalletAsync(
            request.WalletId,
            cancellationToken);

        var result = _walletOperationService.Withdraw(wallet, request.Amount);

        await _unitOfWork.ExecuteAsync(async ct =>
        {
            await _walletRepository.UpdateAsync(wallet, ct);
            await _transactionRepository.AddAsync(result.transaction, ct);
        }, cancellationToken);

        return new WithdrawResponse(
            wallet.Id,
            wallet.Balance);
    }
}
