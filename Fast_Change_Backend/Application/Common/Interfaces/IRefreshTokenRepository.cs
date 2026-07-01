using Domain.Entities;

namespace Application.Common.Interfaces;

/// <summary>
/// Refresh token repository interface for managing refresh tokens in the application.
/// </summary>
public interface IRefreshTokenRepository
{
    /// <summary>
    /// Gets a user by their unique identifier asynchronously.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken);
}
