using FluentValidation;

namespace Application.Features.Wallets.Withdraw;

public class WithdrawCommandValidator
    : AbstractValidator<WithdrawCommand>
{
    public WithdrawCommandValidator()
    {
        RuleFor(x => x.WalletId)
            .NotEmpty();

        RuleFor(x => x.Amount)
            .GreaterThan(0);
    }
}
