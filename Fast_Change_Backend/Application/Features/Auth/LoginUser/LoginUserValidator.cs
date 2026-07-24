using FluentValidation;
using Resources;

namespace Application.Features.Auth.LoginUser;

public sealed class LoginUserValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(Localization.EmailAddressIsRequired)
            .EmailAddress().WithMessage(Localization.ValidEmailAddressIsRequired)
            .MaximumLength(256).WithMessage(Localization.EmailAddressLength);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(Localization.PasswordIsRequired)
            .MaximumLength(100).WithMessage(Localization.PasswordLength);
    }
}
