using Application.Common.Models;
using Application.Features.Auth.LoginUser;
using Application.Features.Auth.RefreshToken;
using Application.Features.Users.CurrentUser;
using Domain.Entities;
using Mapster;

namespace Application.Common.Mapping;

public sealed class AuthMappings
    : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AuthTokens, LoginUserResponse>();
        config.NewConfig<AuthTokens, RefreshTokenResponse>();
        config.NewConfig<User, GetCurrentUserResponse>();
    }
}
