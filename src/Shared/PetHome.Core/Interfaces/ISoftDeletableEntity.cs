namespace PetHome.Core.Interfaces;
public interface ISoftDeletableEntity
{
    public void SoftDelete();
    public void SoftRestore();
}
