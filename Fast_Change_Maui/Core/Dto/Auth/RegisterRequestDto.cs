namespace Core.Dto.Auth;

/// <summary>
/// Data transfer object for user registration requests.
/// </summary>
public sealed record RegisterRequestDto(string Email, string Password);