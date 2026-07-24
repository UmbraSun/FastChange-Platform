using FluentValidation;
using Resources;

namespace Application.Features.Auth.RefreshToken;

public sealed class RefreshTokenValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage(Localization.RefreshTokenIsRequired);
    }
}
