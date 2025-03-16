using PetHome.Species.Application.Database;
using PetHome.Species.Contracts.Contracts.Species;

namespace PetHome.Species.Application.Features.Contracts.Species;
public class GetSpeciesIdUsingContract : IGetSpeciesIdContract
{
    private readonly ISpeciesRepository _repository;

    public GetSpeciesIdUsingContract(
        ISpeciesRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid?> Execute(string name, CancellationToken ct)
    {
        var result = await _repository.GetByName(name, ct);
        if (result.IsFailure)
            return null;

        var id = result.Value.Id.Value;
        return id;
    }
}
