namespace Core.DTOs.Common;

/// <summary>
/// Represents standard RFC 7807 problem details response from the backend API.
/// </summary>
public sealed record ProblemDetailsDto(
    string Type,
    string Title,
    int Status,
    string Detail,
    string Instance,
    Dictionary<string, string[]>? Errors);
