using Application.Common.Models;

namespace Application.Common.Interfaces;

/// <summary>
/// Jwt token validator interface
/// </summary>
public interface IJwtTokenValidator
{
    /// <summary>
    /// Validates the refresh token and returns the validated refresh token object
    /// </summary>
    /// <param name="refreshToken"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<ValidatedRefreshToken> ValidateRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
}
