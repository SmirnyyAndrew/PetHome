using CSharpFunctionalExtensions;
using PetHome.Core.Interfaces.MessageBusManagement;

namespace PetHome.Core.Models;
public abstract class DomainEntity<TId> : Entity<TId> where TId : IComparable<TId>
{
    protected DomainEntity(TId id) : base(id) { }

    private readonly List<IDomainEvent> _domainEvents = [];
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    protected void RemoveDomainEvent(IDomainEvent domainEvent) => _domainEvents.Remove(domainEvent);
    protected void ClearDomainEvents() => _domainEvents.Clear();
}
