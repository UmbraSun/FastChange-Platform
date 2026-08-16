using MediatR;

namespace Application.Features.Portfolio.GetPortfolio;

public sealed record GetPortfolioQuery(
    string Currency = "USD")
    : IRequest<PortfolioResponse>;
