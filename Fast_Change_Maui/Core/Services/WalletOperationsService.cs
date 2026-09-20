using Core.Api.Wallets;
using Core.DTOs.Wallets;
using Core.Interfaces;

namespace Core.Services;

public sealed class WalletOperationsService : IWalletOperationsService
{
    private readonly IWalletOperationsApi _walletOperationsApi;

    public WalletOperationsService(IWalletOperationsApi walletOperationsApi)
    {
        _walletOperationsApi = walletOperationsApi;
    }

    public Task<DepositResponseDto> DepositAsync(Guid walletId, decimal amount, CancellationToken cancellationToken = default)
    {
        return _walletOperationsApi.DepositAsync(new DepositRequestDto(walletId, amount), cancellationToken);
    }

    public Task<WithdrawResponseDto> WithdrawAsync(Guid walletId, decimal amount, CancellationToken cancellationToken = default)
    {
        return _walletOperationsApi.WithdrawAsync(new WithdrawRequestDto(walletId, amount), cancellationToken);
    }
}