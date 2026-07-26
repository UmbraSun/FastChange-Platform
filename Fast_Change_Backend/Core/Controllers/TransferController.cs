using Application.Features.Transfer.TransferFunds;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Core.Controllers;

[ApiController]
[Route("api/transfers")]
[Authorize]
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
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Transfer(
        [FromBody] TransferCommand command,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            command,
            cancellationToken);

        return Ok(response);
    }
}
