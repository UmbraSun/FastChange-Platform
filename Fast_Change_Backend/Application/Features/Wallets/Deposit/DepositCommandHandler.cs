using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Features.Wallets.Deposit;

public class DepositCommandHandler : IRequestHandler<DepositCommand, DepositResponse>
{
    private readonly IWalletRepository _walletRepository;

    public DepositCommandHandler(
        IWalletRepository walletRepository)
    {
        _walletRepository = walletRepository;
    }

    public async Task<DepositResponse> Handle(
        DepositCommand request,
        CancellationToken cancellationToken)
    {
        var wallet = await _walletRepository.GetByIdAsync(
            request.WalletId,
            cancellationToken);

        if (wallet is null)
            throw new BusinessException("Wallet not found");

        wallet.Deposit(request.Amount);

        await _walletRepository.SaveChangesAsync(
            cancellationToken);

        return new DepositResponse(
            wallet.Id,
            wallet.Balance);
    }
}
