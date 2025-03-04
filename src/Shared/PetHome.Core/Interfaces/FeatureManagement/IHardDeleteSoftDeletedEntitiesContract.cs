namespace PetHome.Core.Interfaces.FeatureManagment;
public interface IHardDeleteSoftDeletedEntitiesContract
{
    public Task HardDeleteExpiredSoftDeletedEntities(CancellationToken ct);
}
