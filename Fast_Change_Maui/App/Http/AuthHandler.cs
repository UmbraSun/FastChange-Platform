using System.Net;
using System.Net.Http.Headers;
using Core.Api.Auth;
using Core.DTOs.Auth;
using Core.Interfaces;

namespace App.Http;

public sealed class AuthHandler : DelegatingHandler
{
    private readonly ITokenStorage _tokenStorage;
    private readonly IAuthApi _authApi;
    private readonly SemaphoreSlim _refreshLock = new(1, 1);

    public AuthHandler(
        ITokenStorage tokenStorage,
        IAuthApi authApi)
    {
        _tokenStorage = tokenStorage;
        _authApi = authApi;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        await AddAccessTokenAsync(request, cancellationToken);

        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode != HttpStatusCode.Unauthorized) return response;

        response.Dispose();

        var refreshed = await TryRefreshAsync(cancellationToken);

        if (!refreshed)
            return new HttpResponseMessage(HttpStatusCode.Unauthorized) { RequestMessage = request };

        var retryRequest = await CloneRequestAsync(request, cancellationToken);
        await AddAccessTokenAsync(retryRequest, cancellationToken);

        return await base.SendAsync(retryRequest, cancellationToken);
    }

    private async Task AddAccessTokenAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var accessToken = await _tokenStorage.GetAccessTokenAsync(cancellationToken);

        if (string.IsNullOrWhiteSpace(accessToken)) return;

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
    }

    private async Task<bool> TryRefreshAsync(
        CancellationToken cancellationToken)
    {
        await _refreshLock.WaitAsync(cancellationToken);

        try
        {
            var refreshToken = await _tokenStorage.GetRefreshTokenAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                await _tokenStorage.ClearAsync(cancellationToken);
                return false;
            }

            var response = await _authApi.RefreshAsync(new RefreshTokenRequestDto(refreshToken), cancellationToken);

            await _tokenStorage.SaveTokensAsync(response.AccessToken, response.RefreshToken, cancellationToken);

            return true;
        }
        catch
        {
            await _tokenStorage.ClearAsync(cancellationToken);
            return false;
        }
        finally
        {
            _refreshLock.Release();
        }
    }

    private static async Task<HttpRequestMessage> CloneRequestAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var clone = new HttpRequestMessage(request.Method, request.RequestUri);
        foreach (var header in request.Headers)
            clone.Headers.TryAddWithoutValidation(header.Key, header.Value);

        if (request.Content is not null)
        {
            var content = await request.Content.ReadAsByteArrayAsync(cancellationToken);
            clone.Content = new ByteArrayContent(content);

            foreach (var header in request.Content.Headers)
                clone.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }

        return clone;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing) _refreshLock.Dispose();

        base.Dispose(disposing);
    }
}