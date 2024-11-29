using CSharpFunctionalExtensions;

namespace PetHome.Domain.PetEntity;
public class Breed
{
    // Для EF core
    public Breed() { }

    public BreedId Id { get; private set; }
    public BreedName Name { get; private set; }
    public SpeciesId SpeciesId { get; private set; }

}
