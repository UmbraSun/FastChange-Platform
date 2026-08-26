using Application.Features.Portfolio.GetMarketOverview;
using Application.Features.Portfolio.GetPortfolio;
using Application.Features.Portfolio.GetPortfolioPerformance;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Core.Controllers;

/// <summary>
/// Controller responsible for handling portfolio-related endpoints, such as retrieving the current portfolio value and its breakdown across different wallets.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class PortfolioController : ControllerBase
{
    private readonly ISender _sender;

    public PortfolioController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Gets the current portfolio value converted to the specified currency.
    /// </summary>
    /// <param name="currency">
    /// The currency in which the total portfolio value should be calculated.
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetPortfolio(
        [FromQuery] string currency = "USD",
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetPortfolioQuery(currency), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Gets the performance of the portfolio, including the current value, previous value, change amount, and change percentage, converted to the specified currency.
    /// </summary>
    /// <param name="currency"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("performance")]
    public async Task<IActionResult> GetPerformance(
        [FromQuery] string currency = "USD",
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetPortfolioPerformanceQuery(currency), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Gets current market prices and 24-hour price changes for the requested cryptocurrencies.
    /// </summary>
    /// <param name="currencies"></param>
    /// <param name="currency"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("market")]
    public async Task<IActionResult> GetMarketOverview(
        [FromQuery] IReadOnlyCollection<string> currencies,
        [FromQuery] string currency = "USD",
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetMarketOverviewQuery(currencies, currency), cancellationToken);
        return Ok(result);
    }
}
