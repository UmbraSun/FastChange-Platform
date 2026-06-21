using Application.Common.Models;

namespace Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    AuthTokens GenerateTokens(Guid userId, string email);
}
