using Application.Common.Models;
using Application.Features.Transfer.SearchRecipients;
using Application.Features.Transfer.TransferFunds;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Core.Controllers;

[ApiController]
[Route("api/transfers")]
[Authorize]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status409Conflict)]
[ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public sealed class TransferController : ControllerBase
{
    private readonly IMediator _mediator;

    public TransferController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Transfers funds between users.
    /// </summary>
    /// <param name="command">Transfer details</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Transfer result</returns>
    [HttpPost]
    [ProducesResponseType(typeof(TransferResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Transfer(
        [FromBody] TransferCommand command,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(command, cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Searches for transfer recipients based on a query and currency.
    /// </summary>
    /// <param name="query"></param>
    /// <param name="currency"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("recipients")]
    [ProducesResponseType(typeof(IReadOnlyList<TransferRecipient>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchRecipients(
        [FromQuery] string query,
        [FromQuery] string currency,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new SearchTransferRecipientsQuery(query, currency), cancellationToken);
        return Ok(result);
    }
}
