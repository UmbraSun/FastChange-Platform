using Core.Api.Exchange;
using Core.DTOs.Exchange;
using Core.Interfaces;

namespace Core.Services;

public sealed class ExchangeService : IExchangeService
{
    private readonly IExchangeApi _exchangeApi;

    public ExchangeService(IExchangeApi exchangeApi)
    {
        _exchangeApi = exchangeApi;
    }

    public Task<PreviewExchangeResponseDto> PreviewAsync(Guid fromWalletId, Guid toWalletId, decimal amount, CancellationToken cancellationToken = default)
    {
        return _exchangeApi.PreviewAsync(fromWalletId, toWalletId, amount, cancellationToken);
    }

    public Task<ExchangeResponseDto> ExchangeAsync(Guid fromWalletId, Guid toWalletId, decimal amount, CancellationToken cancellationToken = default)
    {
        return _exchangeApi.ExchangeAsync(new ExchangeRequestDto(fromWalletId, toWalletId, amount), cancellationToken);
    }
}