using Domain.Entities;

namespace Application.Features.Auth.LoginUser;

public interface ILoginUserRepository
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
}
