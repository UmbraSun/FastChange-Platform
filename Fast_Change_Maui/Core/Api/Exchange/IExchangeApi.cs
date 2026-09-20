using Core.DTOs.Exchange;
using Refit;

namespace Core.Api.Exchange;

/// <summary>
/// Defines the interface for exchange-related API operations, including previewing and executing currency exchanges.
/// </summary>
public interface IExchangeApi
{
    /// <summary>
    /// Previews the exchange between two wallets, providing details such as the exchange rate, sent amount, and received amount.
    /// </summary>
    /// <param name="fromWalletId"></param>
    /// <param name="toWalletId"></param>
    /// <param name="amount"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Get("/api/exchange/preview")]
    Task<PreviewExchangeResponseDto> PreviewAsync(
        Guid fromWalletId,
        Guid toWalletId,
        decimal amount,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes the exchange between two wallets, transferring the specified amount and returning the resulting balances and exchange details.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Post("/api/exchange")]
    Task<ExchangeResponseDto> ExchangeAsync(
        [Body] ExchangeRequestDto request,
        CancellationToken cancellationToken = default);
}
