using Domain.Entities;

namespace Application.Common.Interfaces;

/// <summary>
/// Repository contract for user registration data operations.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Retrieves a user by their unique identifier from PostgreSQL
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Determines if the email already exists in PostgreSQL
    /// </summary>
    /// <param name="email"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> IsEmailTakenAsync(string email, CancellationToken cancellationToken);

    /// <summary>
    /// Saves the completely assembled user and wallet state graph
    /// </summary>
    /// <param name="user"></param>
    /// <param name="wallets"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task SaveUserWithWalletsAsync(User user, List<Wallet> wallets, CancellationToken cancellationToken);
}
