using Application.Common.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Core.Infrastructure;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid UserId =>
        Guid.Parse(
            _httpContextAccessor.HttpContext?
                .User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);

    public string? Email =>
        _httpContextAccessor.HttpContext?
            .User.FindFirstValue(JwtRegisteredClaimNames.Email);
}
