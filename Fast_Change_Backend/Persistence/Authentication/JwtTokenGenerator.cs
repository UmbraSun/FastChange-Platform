using Application.Common.Authentication;
using System.Text;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Persistence.Authentication;

public sealed class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtSettings _jwtSettings;
    private readonly JsonWebTokenHandler _tokenHandler;

    public JwtTokenGenerator(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
        _tokenHandler = new JsonWebTokenHandler();
    }

    public AuthTokens GenerateTokens(Guid userId, string email)
    {
        var accessToken = CreateToken(
            userId,
            email,
            DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            JwtClaimTypes.Access);

        var refreshToken = CreateToken(
            userId,
            email,
            DateTime.UtcNow.AddDays(_jwtSettings.RefreshExpiryDays),
            JwtClaimTypes.Refresh);

        return new AuthTokens(accessToken, refreshToken);
    }

    private string CreateToken(Guid userId, string email, DateTime expiresAtUtc, string tokenType)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new Dictionary<string, object>
        {
            { JwtRegisteredClaimNames.Sub, userId.ToString() },
            { JwtRegisteredClaimNames.Email, email },
            { JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString() },
            { JwtClaimTypes.TokenType, tokenType }
        };

        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            Claims = claims,
            Expires = expiresAtUtc,
            SigningCredentials = credentials
        };

        return _tokenHandler.CreateToken(descriptor);
    }
}
