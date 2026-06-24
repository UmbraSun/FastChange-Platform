using FluentValidation;

namespace Application.Features.Wallets.Deposit;

public class DepositCommandValidator : AbstractValidator<DepositCommand>
{
    public DepositCommandValidator()
    {
        RuleFor(x => x.Amount)
            .GreaterThan(0);

        RuleFor(x => x.WalletId)
            .NotEmpty();
    }
}