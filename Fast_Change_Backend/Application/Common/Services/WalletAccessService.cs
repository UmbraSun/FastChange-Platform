using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Resources;

namespace Application.Common.Services;

public sealed class WalletAccessService : IWalletAccessService
{
    private readonly IWalletRepository _walletRepository;
    private readonly ICurrentUserService _currentUserService;

    public WalletAccessService(
        IWalletRepository walletRepository,
        ICurrentUserService currentUserService)
    {
        _walletRepository = walletRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Wallet> GetOwnedWalletAsync(
        Guid walletId,
        CancellationToken cancellationToken)
    {
        var wallet = await _walletRepository.GetByIdAsync(
            walletId,
            cancellationToken);

        if (wallet is null)
            throw new BusinessException(Localization.WalletNotFound);

        if (wallet.UserId != _currentUserService.UserId)
            throw new BusinessException(
                Localization.WalletIsNotAssociatedWithThisUser);

        return wallet;
    }

    public async Task<IReadOnlyList<Wallet>> GetOwnedWalletsAsync(
        CancellationToken cancellationToken)
    {
        return await _walletRepository.GetByUserIdAsync(
            _currentUserService.UserId,
            cancellationToken);
    }

    public Task EnsureAccessAsync(
        Wallet wallet,
        CancellationToken cancellationToken = default)
    {
        if (wallet.UserId != _currentUserService.UserId)
            throw new BusinessException(
                Localization.WalletIsNotAssociatedWithThisUser);

        return Task.CompletedTask;
    }
}
