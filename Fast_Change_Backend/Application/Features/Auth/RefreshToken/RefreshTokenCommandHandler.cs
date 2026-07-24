using Application.Common.Interfaces;
using MediatR;
using Resources;

namespace Application.Features.Auth.RefreshToken;

/// <summary>
/// Validates a refresh token and issues a new access and refresh token pair.
/// </summary>
public sealed class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResponse>
{
    private readonly IJwtTokenValidator _jwtTokenValidator;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public RefreshTokenCommandHandler(
        IJwtTokenValidator jwtTokenValidator,
        IJwtTokenGenerator jwtTokenGenerator,
        IRefreshTokenRepository refreshTokenRepository)
    {
        _jwtTokenValidator = jwtTokenValidator;
        _jwtTokenGenerator = jwtTokenGenerator;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<RefreshTokenResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var validatedToken = await _jwtTokenValidator.ValidateRefreshTokenAsync(request.RefreshToken, cancellationToken);

        var user = await _refreshTokenRepository.GetByIdAsync(validatedToken.UserId, cancellationToken);

        if (user is null || user.Email != validatedToken.Email)
            throw new UnauthorizedAccessException(Localization.InvalidRefreshToken);

        var tokens = _jwtTokenGenerator.GenerateTokens(user.Id, user.Email);

        return new RefreshTokenResponse(tokens.AccessToken, tokens.RefreshToken);
    }
}
