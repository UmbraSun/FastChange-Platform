using Application.Features.Exchange.ExchangeCurrency;
using Application.Features.Exchange.PreviewExchange;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Core.Controllers;

/// <summary>
/// Exchange Controller handles exchange-related operations such as previewing exchange rates and performing currency exchanges.
/// </summary>
[ApiController]
[Route("api/exchange")]
[Authorize]
[EnableRateLimiting("ExchangePolicy")]
public sealed class ExchangeController : ControllerBase
{
    private readonly IMediator _mediator;

    public ExchangeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Previews the exchange rate and calculates the target amount based on the provided source currency, target currency, and amount.
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("preview")]
    public async Task<IActionResult> Preview(
        [FromQuery] PreviewExchangeQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            query,
            cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Performs currency exchange operation.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Exchange(
        ExchangeCommand command,
        CancellationToken cancellationToken)
    {
        var result =
            await _mediator.Send(
                command,
                cancellationToken);

        return Ok(result);
    }
}
