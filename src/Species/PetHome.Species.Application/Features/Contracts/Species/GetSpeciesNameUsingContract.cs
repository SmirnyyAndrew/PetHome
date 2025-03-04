using PetHome.Species.Application.Database;
using PetHome.Species.Contracts.Contracts.Species;

namespace PetHome.Species.Application.Features.Contracts.Species;
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
