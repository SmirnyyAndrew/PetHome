using CSharpFunctionalExtensions;

namespace PetHome.Domain.PetManagment.PetEntity;
public class Breed
{
    private Breed() { }

    public BreedId Id { get; private set; }
    public BreedName Name { get; private set; }
    public SpeciesId SpeciesId { get; private set; }
}
