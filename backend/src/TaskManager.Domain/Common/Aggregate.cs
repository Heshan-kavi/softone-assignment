namespace TaskManager.Domain.Common;

public abstract class Aggregate : Auditable
{
    private readonly List<IDomainEvent> _domainEvents = new();

    protected Aggregate(Guid id) : base(id) { }
    protected Aggregate() { }

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void RaiseDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    public void ClearDomainEvents() => _domainEvents.Clear();
}
