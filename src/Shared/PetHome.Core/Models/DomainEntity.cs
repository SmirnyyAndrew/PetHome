using CSharpFunctionalExtensions;
using PetHome.Core.Interfaces.MessageBusManagement;

namespace PetHome.Core.Models;
public abstract class DomainEntity<TId> : Entity<TId> where TId : IComparable<TId>
{
    protected DomainEntity(TId id) : base(id) { }

    private readonly List<IDomainEvent> _domainEvents = [];
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    public void RemoveDomainEvent(IDomainEvent domainEvent) => _domainEvents.Remove(domainEvent);
    public void ClearDomainEvents() => _domainEvents.Clear();
}
