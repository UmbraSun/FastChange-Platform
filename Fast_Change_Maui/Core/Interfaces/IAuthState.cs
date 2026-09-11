namespace Core.Interfaces;

/// <summary>
/// Represents the authentication state of the application.
/// </summary>
public interface IAuthState
{
    /// <summary>
    /// Checks if the user is authenticated.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> IsAuthenticatedAsync(CancellationToken cancellationToken = default);
}