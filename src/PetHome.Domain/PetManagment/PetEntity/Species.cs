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
    public List<Breed> Breeds { get; private set; } = new List<Breed>();

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
        List<Breed> newBreads = new List<Breed>();
        foreach (var breed in breedNames)
        {
            var createBreedResult = Breed.Create(breed, Id.Value);
            bool isNotUniqueBreed = Breeds.Any(x => x.Name.Value.ToLower() == breed.ToLower());
            if (isNotUniqueBreed)
                return Errors.Conflict($"Порода с именем {breed} уже существует");

            if (createBreedResult.IsFailure)
                return createBreedResult.Error;

            newBreads.Add(createBreedResult.Value);
        }
        Breeds.AddRange(newBreads);
        return Result.Success<Error>();
    } 
}
