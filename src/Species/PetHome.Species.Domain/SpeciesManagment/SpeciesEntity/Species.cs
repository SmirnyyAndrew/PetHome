using CSharpFunctionalExtensions;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Species.Domain.SpeciesManagment.BreedEntity;

namespace PetHome.Species.Domain.SpeciesManagment.SpeciesEntity;
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
    private List<Breed> _breeds = [];
    public IReadOnlyList<Breed> Breeds => _breeds;

    public static Result<Species, Error> Create(string name)
    {
        var speciesIdResult = SpeciesId.Create();
        var speciesNameResult = SpeciesName.Create(name);
        if (speciesIdResult.IsFailure || speciesNameResult.IsFailure)
            return speciesNameResult.Error;

        return new Species(speciesIdResult.Value, speciesNameResult.Value);
    }

    public UnitResult<Error> UpdateBreeds(IEnumerable<string> breedNames)
    {
        foreach (var breed in breedNames)
        {
            bool isNotUniqueBreed = Breeds.Any(x => x.Name.Value.ToLower() == breed.ToLower());
            if (isNotUniqueBreed)
                return Errors.Conflict($"Порода с именем {breed} уже существует");

            var createBreedResult = Breed.Create(breed, Id.Value);
            if (createBreedResult.IsFailure)
                return createBreedResult.Error;

            _breeds.Add(createBreedResult.Value);
        }
        _breeds.AddRange(Breeds);
        return Result.Success<Error>();
    }
}
