using Domain.Entities;

namespace Application.Features.Users.RegisterUser;

/// <summary>
/// Repository contract for user registration data operations.
/// </summary>
public interface IUserRepository
{
    // Determines if the email already exists in PostgreSQL
    Task<bool> IsEmailTakenAsync(string email, CancellationToken cancellationToken);

    // Saves the completely assembled user and wallet state graph
    Task SaveUserWithWalletsAsync(User user, List<Wallet> wallets, CancellationToken cancellationToken);
}
