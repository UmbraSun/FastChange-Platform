using Core.Interfaces;

namespace App.Services;

public sealed class AuthState : IAuthState
{
    private readonly ITokenStorage _tokenStorage;

    public AuthState(ITokenStorage tokenStorage)
    {
        _tokenStorage = tokenStorage;
    }

    public async Task<bool> IsAuthenticatedAsync(CancellationToken cancellationToken = default)
    {
        var accessToken = await _tokenStorage.GetAccessTokenAsync(cancellationToken);
        return !string.IsNullOrWhiteSpace(accessToken);
    }
}