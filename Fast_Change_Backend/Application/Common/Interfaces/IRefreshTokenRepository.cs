using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IRefreshTokenRepository
{
    Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken);
}
