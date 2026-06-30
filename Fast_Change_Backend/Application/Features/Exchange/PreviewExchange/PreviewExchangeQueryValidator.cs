using FluentValidation;

namespace Application.Features.Exchange.PreviewExchange;

public sealed class PreviewExchangeQueryValidator
    : AbstractValidator<PreviewExchangeQuery>
{
    public PreviewExchangeQueryValidator()
    {
        RuleFor(x => x.Amount)
            .GreaterThan(0);
    }
}
