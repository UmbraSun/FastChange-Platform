namespace Core.Dto.Auth;

/// <summary>
/// Data transfer object representing the successful registration response.
/// </summary>
public sealed record RegisterResponseDto(Guid UserId, string Message);
