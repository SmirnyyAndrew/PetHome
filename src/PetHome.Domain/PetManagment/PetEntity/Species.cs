using CSharpFunctionalExtensions;
using PetHome.Domain.Shared.Error;

namespace PetHome.Domain.PetManagment.PetEntity;
public class Species
{
    public Species() { }
    private Species(
        SpeciesId id,
        SpeciesName name)
    {
        Id = id;
        Name = name;
    }
    public SpeciesId Id { get; private set; }
    public SpeciesName Name { get; private set; }
    public IReadOnlyList<Breed> Breeds { get; private set; } = new List<Breed>();

    public static Result<Species, Error> Create(string name)
    {
        var speciesIdResult = SpeciesId.Create();
        var speciesNameResult = SpeciesName.Create(name);
        if (speciesIdResult.IsFailure || speciesNameResult.IsFailure)
            return speciesNameResult.Error;

        return new Species(speciesIdResult.Value, speciesNameResult.Value);
    }

    public Result<Guid, Error> UpdateBreeds(IEnumerable<Breed> breeds)
    {
        Breeds = breeds.ToList();
        return Id.Value;
    }
}
