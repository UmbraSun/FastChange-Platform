using Core.Api.Portfolio;
using Core.DTOs.Portfolio;
using Core.Interfaces;

namespace Core.Services;

public sealed class PortfolioService : IPortfolioService
{
    private readonly IPortfolioApi _portfolioApi;

    public PortfolioService(IPortfolioApi portfolioApi)
    {
        _portfolioApi = portfolioApi;
    }

    public Task<PortfolioResponseDto> GetPortfolioAsync(string currency = "USD", CancellationToken cancellationToken = default)
    {
        return _portfolioApi.GetPortfolioAsync(currency, cancellationToken);
    }

    public Task<PortfolioPerformanceResponseDto> GetPerformanceAsync(string currency = "USD", CancellationToken cancellationToken = default)
    {
        return _portfolioApi.GetPerformanceAsync(currency, cancellationToken);
    }
}