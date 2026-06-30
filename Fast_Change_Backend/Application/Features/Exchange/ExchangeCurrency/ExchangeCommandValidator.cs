using FluentValidation;
using Resources;

namespace Application.Features.Exchange.ExchangeCurrency;

public sealed class ExchangeCommandValidator
    : AbstractValidator<ExchangeCommand>
{
    public ExchangeCommandValidator()
    {
        RuleFor(x => x.FromWalletId)
            .NotEmpty();

        RuleFor(x => x.ToWalletId)
            .NotEmpty();

        RuleFor(x => x.Amount)
            .GreaterThan(0);

        RuleFor(x => x)
            .Must(x => x.FromWalletId != x.ToWalletId)
            .WithMessage(Localization.SourceAndDestinationWalletsMustBeDifferent);
    }
}
