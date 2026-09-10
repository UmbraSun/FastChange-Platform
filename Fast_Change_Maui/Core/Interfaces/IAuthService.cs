using Core.DTOs.Auth;

namespace Core.Interfaces;

/// <summary>
/// Defines the interface for authentication-related services, including user registration, login, token refresh, and logout operations.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Registers a new user with the provided registration details.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Logs in a user with the provided login credentials and returns an authentication response containing access and refresh tokens.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<LoginResponseDto> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Refreshes the access token using the provided refresh token and returns a new authentication response containing updated access and refresh tokens.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<RefreshTokenResponseDto> RefreshAsync(RefreshTokenRequestDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Logs out the currently authenticated user, invalidating their session and tokens.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task LogoutAsync(CancellationToken cancellationToken = default);
}
