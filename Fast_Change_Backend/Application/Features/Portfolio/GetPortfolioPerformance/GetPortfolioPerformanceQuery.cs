using MediatR;

namespace Application.Features.Portfolio.GetPortfolioPerformance;

public sealed record GetPortfolioPerformanceQuery(string Currency = "USD")
    : IRequest<PortfolioPerformanceResponse>;
