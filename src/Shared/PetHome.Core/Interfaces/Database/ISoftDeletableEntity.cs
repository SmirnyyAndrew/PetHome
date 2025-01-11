namespace PetHome.Core.Interfaces.Database;
public interface ISoftDeletableEntity
{
    public DateTime DeletionDate { get; set; }
    public bool IsDeleted { get; set; }

    public void SoftDelete();
    public void SoftRestore();
}
