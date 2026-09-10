namespace Core.Interfaces;

/// <summary>
/// Represents a storage mechanism for access and refresh tokens.
/// </summary>
public interface ITokenStorage
{
    /// <summary>
    /// Gets the access token from the storage.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<string?> GetAccessTokenAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the refresh token from the storage.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<string?> GetRefreshTokenAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Saves the access and refresh tokens to the storage.
    /// </summary>
    /// <param name="accessToken"></param>
    /// <param name="refreshToken"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task SaveTokensAsync(string accessToken, string refreshToken, CancellationToken cancellationToken = default);

    /// <summary>
    /// Clears the access and refresh tokens from the storage.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task ClearAsync(CancellationToken cancellationToken = default);
}