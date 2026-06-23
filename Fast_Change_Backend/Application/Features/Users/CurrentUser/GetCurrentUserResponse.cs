namespace Application.Features.Users.CurrentUser;

public sealed record GetCurrentUserResponse(
    Guid Id,
    string Email,
    bool IsVerified);
