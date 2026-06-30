using Domain.Common;

namespace Domain.Events;

public sealed class WalletDepositedDomainEvent : DomainEvent
{
    public Guid WalletId { get; }
    public decimal Amount { get; }

    public WalletDepositedDomainEvent(Guid walletId, decimal amount)
    {
        WalletId = walletId;
        Amount = amount;
    }
}
