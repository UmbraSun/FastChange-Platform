namespace Core.DTOs.Auth;

/// <summary>
/// Represents the response returned after successfully refreshing an authentication token, containing the new access token and refresh token.
/// </summary>
/// <param name="AccessToken"></param>
/// <param name="RefreshToken"></param>
public sealed record RefreshTokenResponseDto(string AccessToken, string RefreshToken);
