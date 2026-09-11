using Core.Interfaces;

namespace App.Services;

public sealed class StartupNavigationService
{
    private readonly IAuthState _authState;

    public StartupNavigationService(IAuthState authState)
    {
        _authState = authState;
    }

    public async Task NavigateAsync(CancellationToken cancellationToken = default)
    {
        var isAuthenticated = await _authState.IsAuthenticatedAsync(cancellationToken);
        var route = isAuthenticated ? "//dashboard" : "//login";
        await Shell.Current.GoToAsync(route);
    }
}