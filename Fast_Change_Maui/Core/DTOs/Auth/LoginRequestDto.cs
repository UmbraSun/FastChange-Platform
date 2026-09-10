namespace Core.DTOs.Auth;

/// <summary>
/// Represents a data transfer object for user login requests, containing the user's email and password.
/// </summary>
/// <param name="Email"></param>
/// <param name="Password"></param>
public sealed record LoginRequestDto(string Email, string Password);
