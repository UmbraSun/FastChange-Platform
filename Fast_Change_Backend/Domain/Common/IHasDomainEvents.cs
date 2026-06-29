namespace Domain.Common;

public interface IHasDomainEvents
{
    public List<DomainEvent> DomainEvents { get; }
    void ClearDomainEvents();
}
