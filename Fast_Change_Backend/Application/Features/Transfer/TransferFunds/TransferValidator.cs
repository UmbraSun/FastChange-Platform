using FluentValidation;
using Resources;

namespace Application.Features.Transfer.TransferFunds;

public sealed class TransferValidator
    : AbstractValidator<TransferCommand>
{
    public TransferValidator()
    {
        RuleFor(x => x.FromWalletId).NotEmpty();
        RuleFor(x => x.ToWalletId).NotEmpty();
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage(Localization.AmountGreaterThanZero);
    }
}
