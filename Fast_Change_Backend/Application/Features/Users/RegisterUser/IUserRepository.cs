using Domain.Entities;

namespace Application.Features.Users.RegisterUser;

/// <summary>
/// Repository contract for user registration data operations.
/// </summary>
public interface IUserRepository
{
    // Retrieves a user by their unique identifier from PostgreSQL
    Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    // Determines if the email already exists in PostgreSQL
    Task<bool> IsEmailTakenAsync(string email, CancellationToken cancellationToken);

    // Saves the completely assembled user and wallet state graph
    Task SaveUserWithWalletsAsync(User user, List<Wallet> wallets, CancellationToken cancellationToken);
}
