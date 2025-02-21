using PetHome.Core.Models;

namespace PetHome.Core.Interfaces.Database;
public abstract class SoftDeletableEntity<TId>
    : DomainEntity<TId>, ISoftDeletableEntity where TId : IComparable<TId>
{
    protected SoftDeletableEntity(TId id) : base(id) { } 

    public DateTime DeletionDate { get; set; }
    public bool IsDeleted { get; set; }

    public virtual void SoftDelete()
    {
        DeletionDate = DateTime.UtcNow;
        IsDeleted = true;
    }
    public virtual void SoftRestore()
    {
        DeletionDate = default;
        IsDeleted = false;
    }
}
