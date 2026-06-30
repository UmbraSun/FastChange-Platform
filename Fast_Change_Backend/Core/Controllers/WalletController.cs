using Application.Features.Transactions.GetTransactionHistory;
using Application.Features.Wallets.Deposit;
using Application.Features.Wallets.Withdraw;
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

    /// <summary>
    /// Deposits balance into the wallet
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("deposit")]
    public async Task<IActionResult> Deposit(
        DepositCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Withdraws balance from the wallet
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("withdraw")]
    public async Task<IActionResult> Withdraw(
        WithdrawCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            command,
            cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Gets transactions for a specific wallet by its ID
    /// </summary>
    /// <param name="walletId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{walletId:guid}/transactions")]
    public async Task<IActionResult> GetTransactions(
        GetTransactionHistoryQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            query,
            cancellationToken);

        return Ok(result);
    }
}
