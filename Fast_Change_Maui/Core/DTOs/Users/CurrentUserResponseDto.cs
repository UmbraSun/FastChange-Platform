namespace Core.DTOs.Users;

public sealed record CurrentUserResponseDto(
    Guid Id,
    string Email,
    bool IsVerified);
