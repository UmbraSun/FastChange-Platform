using Application.Common.Models;

namespace Application.Common.Interfaces;

/// <summary>
/// JWT Token Generator interface for generating authentication tokens.
/// </summary>
public interface IJwtTokenGenerator
{
    /// <summary>
    /// Generates authentication tokens (access and refresh tokens) for a given user ID and email.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="email"></param>
    /// <returns></returns>
    AuthTokens GenerateTokens(Guid userId, string email);
}
