using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Resources;

namespace Application.Features.Wallets.Withdraw;

public class WithdrawCommandHandler
    : IRequestHandler<WithdrawCommand, WithdrawResponse>
{
    private readonly IWalletRepository _walletRepository;
    private readonly ITransactionRepository _transactionRepository;

    public WithdrawCommandHandler(
        IWalletRepository walletRepository,
        ITransactionRepository transactionRepository)
    {
        _walletRepository = walletRepository;
        _transactionRepository = transactionRepository;
    }

    public async Task<WithdrawResponse> Handle(
        WithdrawCommand request,
        CancellationToken cancellationToken)
    {
        var wallet = await _walletRepository.GetByIdAsync(
            request.WalletId,
            cancellationToken);

        if (wallet is null)
            throw new BusinessException(Localization.WalletNotFound);

        wallet.Withdraw(request.Amount);

        var transaction = new Transaction
        {
            WalletId = wallet.Id,
            Currency = wallet.Currency,
            Amount = request.Amount,
            Type = TransactionType.Withdraw
        };

        await _transactionRepository.AddAsync(
            transaction,
            cancellationToken);

        await _walletRepository.SaveChangesAsync(
            cancellationToken);

        return new WithdrawResponse(
            wallet.Id,
            wallet.Balance);
    }
}
