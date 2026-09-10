using Core.Interfaces;

namespace App.Services;

public sealed class SecureTokenStorage : ITokenStorage
{
    private const string AccessTokenKey = "fastchange_access_token";
    private const string RefreshTokenKey = "fastchange_refresh_token";

    public Task<string?> GetAccessTokenAsync(CancellationToken cancellationToken = default)
    {
        return SecureStorage.Default.GetAsync(AccessTokenKey);
    }

    public Task<string?> GetRefreshTokenAsync(CancellationToken cancellationToken = default)
    {
        return SecureStorage.Default.GetAsync(RefreshTokenKey);
    }

    public async Task SaveTokensAsync(
        string accessToken,
        string refreshToken,
        CancellationToken cancellationToken = default)
    {
        await SecureStorage.Default.SetAsync(
            AccessTokenKey,
            accessToken);

        await SecureStorage.Default.SetAsync(
            RefreshTokenKey,
            refreshToken);
    }

    public Task ClearAsync(CancellationToken cancellationToken = default)
    {
        SecureStorage.Default.Remove(AccessTokenKey);
        SecureStorage.Default.Remove(RefreshTokenKey);

        return Task.CompletedTask;
    }
}