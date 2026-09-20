using Core.DTOs.Wallets;

namespace Core.Interfaces;

/// <summary>
/// Defines the interface for wallet operations service, which provides methods for depositing and withdrawing funds from a wallet.
/// </summary>
public interface IWalletOperationsService
{
    /// <summary>
    /// Deposits a specified amount into the wallet identified by the provided wallet ID.
    /// </summary>
    /// <param name="walletId"></param>
    /// <param name="amount"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<DepositResponseDto> DepositAsync(
        Guid walletId,
        decimal amount,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Withdraws a specified amount from the wallet identified by the provided wallet ID.
    /// </summary>
    /// <param name="walletId"></param>
    /// <param name="amount"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WithdrawResponseDto> WithdrawAsync(
        Guid walletId,
        decimal amount,
        CancellationToken cancellationToken = default);
}