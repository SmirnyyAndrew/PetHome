using PetManagementService.Application.Database;
using PetManagementService.Contracts.Contracts.Species;

namespace PetManagementService.Application.Features.Contracts.SpeciesEntity;
public class GetSpeciesNameUsingContract(ISpeciesRepository repository)
    : IGetSpeciesNameContract
{
    public async Task<string?> Execute(Guid Id, CancellationToken ct)
    {
        var getSpeciesResult = await repository.GetById(Id, ct);
        if (getSpeciesResult.IsFailure)
            return null;

        string speciesName = getSpeciesResult.Value.Name;
        return speciesName;
    }
}
