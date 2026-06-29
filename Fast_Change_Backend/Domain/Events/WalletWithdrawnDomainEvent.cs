using Domain.Common;

namespace Domain.Events;

public sealed class WalletWithdrawnDomainEvent : DomainEvent
{
    public Guid WalletId { get; }
    public decimal Amount { get; }

    public WalletWithdrawnDomainEvent(Guid walletId, decimal amount)
    {
        WalletId = walletId;
        Amount = amount;
    }
}
