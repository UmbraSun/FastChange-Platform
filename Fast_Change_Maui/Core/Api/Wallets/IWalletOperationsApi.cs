using Core.DTOs.Wallets;
using Refit;

namespace Core.Api.Wallets;

/// <summary>
/// Represents the API interface for wallet operations, including deposit and withdrawal functionalities.
/// </summary>
public interface IWalletOperationsApi
{
    /// <summary>
    /// Deposits a specified amount into the wallet.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Post("/api/Wallet/deposit")]
    Task<DepositResponseDto> DepositAsync(
        [Body] DepositRequestDto request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Withdraws a specified amount from the wallet.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Post("/api/Wallet/withdraw")]
    Task<WithdrawResponseDto> WithdrawAsync(
        [Body] WithdrawRequestDto request,
        CancellationToken cancellationToken = default);
}