using System.Text;
using Application.Common.Authentication;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Persistence.Authentication;

public sealed class JwtTokenValidator : IJwtTokenValidator
{
    private readonly JwtSettings _jwtSettings;
    private readonly JsonWebTokenHandler _tokenHandler;

    public JwtTokenValidator(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
        _tokenHandler = new JsonWebTokenHandler();
    }

    public async Task<ValidatedRefreshToken> ValidateRefreshTokenAsync(
        string refreshToken,
        CancellationToken cancellationToken = default)
    {
        TokenValidationResult validationResult;

        try
        {
            validationResult = await _tokenHandler.ValidateTokenAsync(refreshToken, CreateValidationParameters());
        }
        catch (SecurityTokenException)
        {
            throw new UnauthorizedAccessException("Invalid refresh token.");
        }

        if (!validationResult.IsValid || validationResult.SecurityToken is not JsonWebToken jsonWebToken)
            throw new UnauthorizedAccessException("Invalid refresh token.");

        var tokenType = jsonWebToken.GetClaim(JwtClaimTypes.TokenType)?.Value;
        if (tokenType != JwtClaimTypes.Refresh)
            throw new UnauthorizedAccessException("Invalid refresh token.");

        var email = jsonWebToken.GetClaim(JwtRegisteredClaimNames.Email)?.Value;
        if (!Guid.TryParse(jsonWebToken.Subject, out var userId) || string.IsNullOrWhiteSpace(email))
            throw new UnauthorizedAccessException("Invalid refresh token.");

        return new ValidatedRefreshToken(userId, email);
    }

    private TokenValidationParameters CreateValidationParameters()
    {
        return new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _jwtSettings.Issuer,
            ValidAudience = _jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret))
        };
    }
}
