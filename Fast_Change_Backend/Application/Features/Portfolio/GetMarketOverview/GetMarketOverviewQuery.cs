using Application.Common.Models;
using MediatR;

namespace Application.Features.Portfolio.GetMarketOverview;

public sealed record GetMarketOverviewQuery(IReadOnlyCollection<string> Currencies, string Currency = "USD")
    : IRequest<IReadOnlyList<MarketDataItem>>;
