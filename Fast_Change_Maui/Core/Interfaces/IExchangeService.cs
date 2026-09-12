using Core.DTOs.Exchange;

namespace Core.Interfaces;

/// <summary>
/// Represents a service for handling currency exchange operations between wallets.
/// </summary>
public interface IExchangeService
{
    /// <summary>
    /// Previews the exchange operation between two wallets, providing details such as the exchange rate, sent amount, and received amount.
    /// </summary>
    /// <param name="fromWalletId"></param>
    /// <param name="toWalletId"></param>
    /// <param name="amount"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<PreviewExchangeResponseDto> PreviewAsync(Guid fromWalletId, Guid toWalletId, decimal amount, CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes the exchange operation between two wallets, transferring the specified amount and returning details of the transaction.
    /// </summary>
    /// <param name="fromWalletId"></param>
    /// <param name="toWalletId"></param>
    /// <param name="amount"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<ExchangeResponseDto> ExchangeAsync(Guid fromWalletId, Guid toWalletId, decimal amount, CancellationToken cancellationToken = default);
}
