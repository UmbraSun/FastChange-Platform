using Core.DTOs.Users;
using Core.DTOs.Wallets;
using Refit;

namespace Core.Api.Users;

/// <summary>
/// Represents the user API interface for interacting with user-related endpoints.
/// </summary>
public interface IUserApi
{
    /// <summary>
    /// Gets the current user's information asynchronously.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Get("/api/User/me")]
    Task<CurrentUserResponseDto> GetCurrentUserAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the wallets associated with the current user asynchronously.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Get("/api/User/wallets")]
    Task<IReadOnlyList<WalletDto>> GetWalletsAsync(CancellationToken cancellationToken = default);
}