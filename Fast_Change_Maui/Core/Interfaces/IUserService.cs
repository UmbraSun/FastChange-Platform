using Core.DTOs.Users;
using Core.DTOs.Wallets;

namespace Core.Interfaces;

/// <summary>
/// Represents a service for managing user-related operations.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Gets the current user's information asynchronously.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<CurrentUserResponseDto> GetCurrentUserAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the wallets associated with the current user asynchronously.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IReadOnlyList<WalletDto>> GetWalletsAsync(CancellationToken cancellationToken = default);
}