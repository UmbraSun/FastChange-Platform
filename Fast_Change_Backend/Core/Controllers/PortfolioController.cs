using Application.Features.Portfolio.GetPortfolio;
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
}
