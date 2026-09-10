using Core.DTOs.Auth;
using Refit;

namespace Core.Api.Auth;

/// <summary>
/// Defines the interface for authentication-related API operations, including user registration, login, and token refresh.
/// </summary>
public interface IAuthApi
{
    /// <summary>
    /// Registers a new user with the provided registration details.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Post("/api/Auth/register")]
    Task<RegisterResponseDto> RegisterAsync([Body] RegisterRequestDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Logs in a user with the provided login credentials and returns an authentication token.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Post("/api/Auth/login")]
    Task<LoginResponseDto> LoginAsync([Body] LoginRequestDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Refreshes the authentication token using the provided refresh token and returns a new authentication token.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Post("/api/Auth/refresh")]
    Task<RefreshTokenResponseDto> RefreshAsync([Body] RefreshTokenRequestDto request, CancellationToken cancellationToken = default);
}