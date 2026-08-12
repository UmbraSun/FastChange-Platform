using Application.Common.Interfaces;
using Contracts.Enums;
using Contracts.Events;
using MediatR;

namespace Application.Features.Wallets.Withdraw;

public sealed class WithdrawCommandHandler
    : IRequestHandler<WithdrawCommand, WithdrawResponse>
{
    private readonly IWalletRepository _walletRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IWalletOperationService _walletOperationService;
    private readonly IWalletAccessService _walletAccessService;
    private readonly IOutboxWriter _outboxWriter;
    private readonly IUnitOfWork _unitOfWork;

    public WithdrawCommandHandler(
        IWalletRepository walletRepository,
        ITransactionRepository transactionRepository,
        IWalletOperationService walletOperationService,
        IWalletAccessService walletAccessService,
        IOutboxWriter outboxWriter,
        IUnitOfWork unitOfWork)
    {
        _walletRepository = walletRepository;
        _transactionRepository = transactionRepository;
        _walletOperationService = walletOperationService;
        _walletAccessService = walletAccessService;
        _outboxWriter = outboxWriter;
        _unitOfWork = unitOfWork;
    }

    public async Task<WithdrawResponse> Handle(
        WithdrawCommand request,
        CancellationToken cancellationToken)
    {
        var wallet = await _walletAccessService.GetOwnedWalletAsync(request.WalletId, cancellationToken);

        var operationId = Guid.NewGuid();
        var result = _walletOperationService.Withdraw(wallet, request.Amount, operationId);

        await _walletRepository.UpdateAsync(wallet, cancellationToken);
        await _transactionRepository.AddAsync(result.transaction, cancellationToken);

        var integrationEvent = new TransactionCompletedEvent(
            operationId,
            wallet.Id,
            Guid.Empty,
            request.Amount,
            null,
            wallet.Currency,
            wallet.Currency,
            TransactionType.Withdraw,
            result.newBalance,
            0m,
            null);

        await _outboxWriter.AddAsync(integrationEvent, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new WithdrawResponse(
            wallet.Id,
            wallet.Balance);
    }
}
