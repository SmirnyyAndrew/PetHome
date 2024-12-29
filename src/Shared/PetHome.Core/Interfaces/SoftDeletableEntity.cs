using PetHome.Domain.PetManagment.GeneralValueObjects;

namespace PetHome.Domain.Shared.Interfaces;
public abstract class SoftDeletableEntity
{
    public DateTime DeletionDate = default;
    public bool _isDeleted = false;
    public virtual void SoftDelete()
    {
        DeletionDate = DateTime.UtcNow;
        _isDeleted = true;
    }
    public virtual void SoftRestore()
    {
        DeletionDate = default;
        _isDeleted = false;
    }
}
