namespace PetHome.Domain.Shared.Interfaces;
public interface ISoftDeletableEntity
{
    public void SoftDelete();
    public void SoftRestore();
}
