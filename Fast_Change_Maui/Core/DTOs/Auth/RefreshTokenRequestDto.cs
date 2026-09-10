namespace Core.DTOs.Auth;

/// <summary>
/// Represents the request to refresh an authentication token using a provided refresh token.
/// </summary>
/// <param name="RefreshToken"></param>
public sealed record RefreshTokenRequestDto(string RefreshToken);
