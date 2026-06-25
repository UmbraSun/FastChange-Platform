using FluentValidation;

namespace Application.Features.Exchange.PreviewExchange;

public sealed class PreviewExchangeQueryValidator
    : AbstractValidator<PreviewExchangeQuery>
{
    public PreviewExchangeQueryValidator()
    {
        RuleFor(x => x.FromCurrency)
            .NotEmpty()
            .MaximumLength(10);

        RuleFor(x => x.ToCurrency)
            .NotEmpty()
            .MaximumLength(10);

        RuleFor(x => x.Amount)
            .GreaterThan(0);
    }
}
