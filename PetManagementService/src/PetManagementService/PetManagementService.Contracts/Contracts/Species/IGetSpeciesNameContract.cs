namespace PetManagementService.Contracts.Contracts.Species;
public interface IGetSpeciesNameContract
{
    public Task<string?> Execute(Guid Id, CancellationToken ct);
}
