using FastChange.Application.Features.Auth.LoginUser;
using FastChange.Application.Features.Auth.RefreshToken;
using FastChange.Application.Features.Users.RegisterUser;using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Core.Controllers;

/// <summary>
/// Authorization controller responsible for handling authentication-related endpoints such as login, registration, and token refresh.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ISender _mediator;

    public AuthController(ISender mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Registers a new user and provisions default multi-currency wallets.
    /// </summary>
    /// <param name="command">User registration details</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Status of the registration process</returns>
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register(
        [FromBody] RegisterUserCommand command,
        CancellationToken cancellationToken)
    {
        var userId = await _mediator.Send(command, cancellationToken);
        return Ok(new { UserId = userId, Message = "User registered successfully with default wallets." });
    }

    /// <summary>
    /// Authenticates a user and returns JWT access and refresh tokens.
    /// </summary>
    /// <param name="command">User login credentials</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>JWT access and refresh tokens</returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login(
        [FromBody] LoginUserCommand command,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(command, cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Exchanges a valid refresh token for a new access and refresh token pair.
    /// </summary>
    /// <param name="command">Refresh token payload</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>New JWT access and refresh tokens</returns>
    [HttpPost("refresh")]
    [ProducesResponseType(typeof(RefreshTokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Refresh(
        [FromBody] RefreshTokenCommand command,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(command, cancellationToken);
        return Ok(response);
    }
}
