using CSharpFunctionalExtensions;
using PetHome.Core.Response.Error;

namespace PetHome.Volunteers.Domain.PetManagment.PetEntity;
public class Breed
{
    private Breed() { }
    private Breed(BreedName name, SpeciesId speciesId)
    {
        Id = BreedId.Create().Value;
        Name = name;
        SpeciesId = speciesId;
    }

    public BreedId Id { get; private set; }
    public BreedName Name { get; private set; }
    public SpeciesId SpeciesId { get; private set; }

    public static Result<Breed, Error> Create(string name, Guid speciesId)
    {
        var nameResult = BreedName.Create(name);
        if (nameResult.IsFailure)
            return nameResult.Error;

        return new Breed(
            nameResult.Value,
            SpeciesId.Create(speciesId).Value);
    }
}
