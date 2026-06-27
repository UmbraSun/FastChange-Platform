using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Resources;

namespace Application.Features.Wallets.Deposit;

public class DepositCommandHandler : IRequestHandler<DepositCommand, DepositResponse>
{
    private readonly IWalletRepository _walletRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly ICurrentUserService _currentUserService;

    public DepositCommandHandler(
        IWalletRepository walletRepository, 
        ITransactionRepository transactionRepository,
        ICurrentUserService currentUserService)
    {
        _walletRepository = walletRepository;
        _transactionRepository = transactionRepository;
        _currentUserService = currentUserService;
    }

    public async Task<DepositResponse> Handle(
        DepositCommand request,
        CancellationToken cancellationToken)
    {
        var wallet = await _walletRepository.GetByIdAsync(
            request.WalletId,
            cancellationToken);

        if (wallet is null)
            throw new BusinessException(Localization.WalletNotFound);

        if(wallet.UserId != _currentUserService.UserId)
            throw new BusinessException(Localization.WalletIsNotAssociatedWithThisUser);

        wallet.Deposit(request.Amount);

        var transaction = Transaction.CreateDeposit(wallet, request.Amount);

        await _transactionRepository.AddAsync(
            transaction,
            cancellationToken);

        await _walletRepository.SaveChangesAsync(cancellationToken);

        return new DepositResponse(
            wallet.Id,
            wallet.Balance);
    }
}
