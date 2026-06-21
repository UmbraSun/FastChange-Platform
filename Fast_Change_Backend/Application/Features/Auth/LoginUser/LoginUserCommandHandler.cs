using Application.Common.Interfaces;
using FastChange.Application.Features.Auth.LoginUser;
using MediatR;
using Resources;

namespace Application.Features.Auth.LoginUser;

/// <summary>
/// Validates user credentials and issues JWT access and refresh tokens on successful login.
/// </summary>
public sealed class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserResponse>
{
    private readonly ILoginUserRepository _loginUserRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginUserCommandHandler(
        ILoginUserRepository loginUserRepository,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _loginUserRepository = loginUserRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<LoginUserResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _loginUserRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (user is null)
            throw new UnauthorizedAccessException(Localization.InvalidEmailOrPass);

        var passwordHash = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(request.Password));

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException(Localization.InvalidEmailOrPass);

        var tokens = _jwtTokenGenerator.GenerateTokens(user.Id, user.Email);

        return new LoginUserResponse(tokens.AccessToken, tokens.RefreshToken);
    }
}
