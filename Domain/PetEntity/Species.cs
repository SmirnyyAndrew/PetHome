using CSharpFunctionalExtensions;

namespace PetHome.Domain.PetEntity;
public class Species
{  
    public Species() { }

    public SpeciesId Id { get; private set; }
    public SpeciesName Name { get; private set; }
    public IReadOnlyList<Breed> BreedList { get; private set; } 
}
