using Application.Features.Users.CurrentUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Core.Controllers;


/// <summary>
/// User controller responsible for handling user-related endpoints such as retrieving user information, updating user details, and managing user accounts.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly ISender _mediator;

    public UserController(ISender mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Information about the currently authenticated user.
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        var response = await _mediator.Send(new GetCurrentUserQuery());
        return Ok(response);
    }
}
