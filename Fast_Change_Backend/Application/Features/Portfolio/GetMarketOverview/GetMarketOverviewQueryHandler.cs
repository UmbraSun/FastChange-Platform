using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;

namespace Application.Features.Portfolio.GetMarketOverview;

public sealed class GetMarketOverviewQueryHandler
    : IRequestHandler<GetMarketOverviewQuery, IReadOnlyList<MarketDataItem>>
{
    private readonly IBatchMarketDataProvider _marketDataProvider;

    public GetMarketOverviewQueryHandler(
        IBatchMarketDataProvider marketDataProvider)
    {
        _marketDataProvider = marketDataProvider;
    }

    public Task<IReadOnlyList<MarketDataItem>> Handle(
        GetMarketOverviewQuery request,
        CancellationToken cancellationToken)
    {
        return _marketDataProvider.GetMarketDataAsync(request.Currencies, request.Currency, cancellationToken);
    }
}
