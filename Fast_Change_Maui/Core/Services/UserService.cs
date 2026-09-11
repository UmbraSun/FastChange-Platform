using Core.Api.Users;
using Core.DTOs.Users;
using Core.DTOs.Wallets;
using Core.Interfaces;

namespace Core.Services;

public sealed class UserService : IUserService
{
    private readonly IUserApi _userApi;

    public UserService(IUserApi userApi)
    {
        _userApi = userApi;
    }

    public Task<CurrentUserResponseDto> GetCurrentUserAsync(CancellationToken cancellationToken = default)
    {
        return _userApi.GetCurrentUserAsync(cancellationToken);
    }

    public Task<IReadOnlyList<WalletDto>> GetWalletsAsync(CancellationToken cancellationToken = default)
    {
        return _userApi.GetWalletsAsync(cancellationToken);
    }
}