using Core.DTOs.Exchange;
using Refit;

namespace Core.Api.Exchange;

public interface IExchangeApi
{
    [Get("/api/exchange/preview")]
    Task<PreviewExchangeResponseDto> PreviewAsync(
        Guid fromWalletId,
        Guid toWalletId,
        decimal amount,
        CancellationToken cancellationToken = default);

    [Post("/api/exchange")]
    Task<ExchangeResponseDto> ExchangeAsync(
        [Body] ExchangeRequestDto request,
        CancellationToken cancellationToken = default);
}
