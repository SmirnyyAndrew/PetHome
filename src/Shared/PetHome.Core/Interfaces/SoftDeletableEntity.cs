
using CSharpFunctionalExtensions;

namespace PetHome.Core.Interfaces;
public abstract class SoftDeletableEntity : Entity, ISoftDeletableEntity
{ 
    public DateTime DeletionDate = default;
    public bool IsDeleted = false;
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
