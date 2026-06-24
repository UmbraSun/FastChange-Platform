namespace Application.Common.Interfaces;

/// <summary>
/// User service to get the current user information
/// </summary>
public interface ICurrentUserService
{
    /// <summary>
    /// Get user id from the current user
    /// </summary>
    Guid UserId { get; }

    /// <summary>
    /// Get user email from the current user
    /// </summary>
    string? Email { get; }
}
