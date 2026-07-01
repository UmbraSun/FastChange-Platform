using Domain.Entities;

namespace Application.Common.Interfaces;

/// <summary>
/// Login user repository interface for retrieving user information based on email.
/// </summary>
public interface ILoginUserRepository
{
    /// <summary>
    /// Gets a user by their email address asynchronously.
    /// </summary>
    /// <param name="email"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
}
