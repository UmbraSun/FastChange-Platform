using Application.Common.Interfaces;
using Contracts.Enums;
using Contracts.Events;
using MediatR;

namespace Application.Features.Wallets.Deposit;

public sealed class DepositCommandHandler 
    : IRequestHandler<DepositCommand, DepositResponse>
{
    private readonly IWalletRepository _walletRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IWalletOperationService _walletOperationService;
    private readonly IWalletAccessService _walletAccessService;
    private readonly IOutboxWriter _outboxWriter;
    private readonly IUnitOfWork _unitOfWork;

    public DepositCommandHandler(
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

    public async Task<DepositResponse> Handle(
        DepositCommand request,
        CancellationToken cancellationToken)
    {
        var wallet = await _walletAccessService.GetOwnedWalletAsync(
            request.WalletId,
            cancellationToken);

        var operationId = Guid.NewGuid();
        var result = _walletOperationService.Deposit(wallet, request.Amount, operationId);
        
        await _walletRepository.UpdateAsync(wallet, cancellationToken);
        await _transactionRepository.AddAsync(result.transaction, cancellationToken);

        var integrationEvent = new TransactionCompletedEvent(
            operationId,
            Guid.Empty,
            wallet.Id,
            request.Amount,
            request.Amount,
            wallet.Currency,
            wallet.Currency,
            TransactionType.Deposit,
            0m,
            result.newBalance);

        await _outboxWriter.AddAsync(integrationEvent, cancellationToken);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new DepositResponse(
            wallet.Id,
            wallet.Balance);
    }
}
