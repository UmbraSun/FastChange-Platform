using Core.Api.Auth;
using Core.DTOs.Auth;
using Core.Interfaces;

namespace Core.Services;

public sealed class AuthService : IAuthService
{
    private readonly IAuthApi _authApi;
    private readonly ITokenStorage _tokenStorage;

    public AuthService(
        IAuthApi authApi,
        ITokenStorage tokenStorage)
    {
        _authApi = authApi;
        _tokenStorage = tokenStorage;
    }

    public Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request, CancellationToken cancellationToken = default)
    {
        return _authApi.RegisterAsync(request, cancellationToken);
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default)
    {
        var response = await _authApi.LoginAsync(request, cancellationToken);
        await _tokenStorage.SaveTokensAsync(response.AccessToken, response.RefreshToken, cancellationToken);
        return response;
    }

    public async Task<RefreshTokenResponseDto> RefreshAsync(RefreshTokenRequestDto request, CancellationToken cancellationToken = default)
    {
        var response = await _authApi.RefreshAsync(request, cancellationToken);
        await _tokenStorage.SaveTokensAsync(response.AccessToken, response.RefreshToken, cancellationToken);
        return response;
    }

    public Task LogoutAsync(CancellationToken cancellationToken = default)
    {
        return _tokenStorage.ClearAsync(cancellationToken);
    }
}