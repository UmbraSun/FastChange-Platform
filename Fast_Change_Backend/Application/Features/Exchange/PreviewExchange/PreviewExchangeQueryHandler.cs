using Application.Common.Interfaces;
using MediatR;

namespace Application.Features.Exchange.PreviewExchange;

public sealed class PreviewExchangeQueryHandler
    : IRequestHandler<
        PreviewExchangeQuery,
        PreviewExchangeResponse>
{
    private readonly IExchangeRateProvider _exchangeRateProvider;

    public PreviewExchangeQueryHandler(
        IExchangeRateProvider exchangeRateProvider)
    {
        _exchangeRateProvider = exchangeRateProvider;
    }

    public async Task<PreviewExchangeResponse> Handle(
        PreviewExchangeQuery request,
        CancellationToken cancellationToken)
    {
        var rate = await _exchangeRateProvider.GetRateAsync(
            request.FromCurrency,
            request.ToCurrency,
            cancellationToken);

        var targetAmount = request.Amount * rate.Rate;

        return new PreviewExchangeResponse(
            rate.FromCurrency,
            rate.ToCurrency,
            request.Amount,
            targetAmount,
            rate.Rate,
            rate.RetrievedAtUtc);
    }
}
