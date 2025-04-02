namespace PetHome.Core.Application.Interfaces.FeatureManagement;
public interface IHardDeleteSoftDeletedEntitiesContract
{
    public Task HardDeleteExpiredSoftDeletedEntities(CancellationToken ct);
}
