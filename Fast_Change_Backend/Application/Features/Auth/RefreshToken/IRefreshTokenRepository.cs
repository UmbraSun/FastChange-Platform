using Domain.Entities;

namespace Application.Features.Auth.RefreshToken;

public interface IRefreshTokenRepository
{
    Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken);
}
