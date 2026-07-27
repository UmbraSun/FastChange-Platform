using Application.Common.Interfaces;
using Contracts.Enums;
using Contracts.Events;
using Contracts.Exceptions;
using Domain.Entities;
using MediatR;
using Resources;

namespace Application.Features.Transfer.TransferFunds;

public sealed class TransferCommandHandler
    : IRequestHandler<TransferCommand, TransferResponse>
{
    private readonly IWalletRepository _walletRepository;
    private readonly IWalletAccessService _walletAccessService;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IOutboxWriter _outboxWriter;
    private readonly IUnitOfWork _unitOfWork;


    public TransferCommandHandler(
        IWalletRepository walletRepository,
        IWalletAccessService walletAccessService,
        ITransactionRepository transactionRepository,
        IOutboxWriter outboxWriter,
        IUnitOfWork unitOfWork)
    {
        _walletRepository = walletRepository;
        _walletAccessService = walletAccessService;
        _transactionRepository = transactionRepository;
        _outboxWriter = outboxWriter;
        _unitOfWork = unitOfWork;
    }


    public async Task<TransferResponse> Handle(
        TransferCommand request,
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

        if (fromWallet.Id == toWallet.Id)
            throw new BusinessException(Localization.SourceAndDestinationWalletsMustBeDifferent);

        await _walletAccessService.EnsureAccessAsync(fromWallet,cancellationToken);
        
        var operationId = Guid.NewGuid();
        fromWallet.Withdraw(request.Amount);
        toWallet.Deposit(request.Amount);

        var withdrawTransaction = Transaction.Create(
            fromWallet,
            request.Amount,
            -request.Amount,
            fromWallet.Balance,
            TransactionType.Transfer,
            operationId);

        var depositTransaction = Transaction.Create(
            toWallet,
            request.Amount,
            request.Amount,
            toWallet.Balance,
            TransactionType.Transfer,
            operationId);

        await _transactionRepository.AddAsync(withdrawTransaction, cancellationToken);
        await _transactionRepository.AddAsync(depositTransaction, cancellationToken);
        
        var integrationEvent = new TransactionCompletedEvent(
            operationId,
            fromWallet.Id,
            toWallet.Id,
            request.Amount,
            null,
            fromWallet.Currency,
            toWallet.Currency,
            TransactionType.Transfer);

        await _outboxWriter.AddAsync(integrationEvent, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return new TransferResponse(
            operationId,
            request.Amount,
            fromWallet.Balance,
            toWallet.Balance);
    }
}
