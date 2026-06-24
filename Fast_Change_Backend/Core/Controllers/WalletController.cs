using Application.Features.Wallets.Deposit;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Core.Controllers;


/// <summary>
/// Wallet controller responsible for handling wallet-related endpoints such as retrieving wallet information, managing wallet transactions, and other wallet operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WalletController : ControllerBase
{
    private readonly IMediator _mediator;

    public WalletController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("deposit")]
    public async Task<IActionResult> Deposit(
        DepositCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }
}
