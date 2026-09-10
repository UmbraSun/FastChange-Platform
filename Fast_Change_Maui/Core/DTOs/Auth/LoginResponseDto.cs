namespace Core.DTOs.Auth;

/// <summary>
/// Represents the response returned after a successful login operation, containing the access token and refresh token.
/// </summary>
/// <param name="AccessToken"></param>
/// <param name="RefreshToken"></param>
public sealed record LoginResponseDto(string AccessToken, string RefreshToken);
