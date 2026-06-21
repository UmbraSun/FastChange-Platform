using FastChange.Application.Features.Users.RegisterUser;
using FluentValidation;
using Resources;

namespace Application.Features.Users.RegisterUser;

public sealed class RegisterUserValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(Localization.EmailAddressIsRequired)
            .EmailAddress().WithMessage(Localization.ValidEmailAddressIsRequired)
            .MaximumLength(256).WithMessage(Localization.EmailAddressLength);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(Localization.PasswordIsRequired)
            .MinimumLength(8).WithMessage(Localization.PasswordMinimumLength)
            .MaximumLength(100).WithMessage(Localization.PasswordLength);
    }
}
