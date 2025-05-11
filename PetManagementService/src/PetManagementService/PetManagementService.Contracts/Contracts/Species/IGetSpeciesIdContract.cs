namespace PetManagementService.Contracts.Contracts.Species;
public interface IGetSpeciesIdContract
{
    public Task<Guid?> Execute(string name, CancellationToken ct);
}
