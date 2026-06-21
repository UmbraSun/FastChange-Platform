using MediatR;

namespace FastChange.Application.Features.Auth.LoginUser;

public sealed record LoginUserCommand(
    string Email,
    string Password) : IRequest<LoginUserResponse>;

public sealed record LoginUserResponse(string AccessToken, string RefreshToken);
