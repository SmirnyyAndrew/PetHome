
using CSharpFunctionalExtensions;

namespace PetHome.Core.Interfaces.Database;
public abstract class SoftDeletableEntity : Entity, ISoftDeletableEntity
{
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
