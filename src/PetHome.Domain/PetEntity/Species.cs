using CSharpFunctionalExtensions;

namespace PetHome.Domain.PetEntity;
public record Species
{ 
    // Для EF core
    private Species() { }

    public SpeciesId Id { get; private set; }
    public SpeciesName Name { get; private set; }
    public IReadOnlyList<Breed> BreedList { get; private set; }

}
