namespace Application.Common.Models;

public sealed record ValidatedRefreshToken(Guid UserId, string Email);
