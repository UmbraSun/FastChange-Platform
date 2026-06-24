using Domain.Entities;

namespace Application.Common.Interfaces;

public interface ILoginUserRepository
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
}
