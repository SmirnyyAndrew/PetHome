namespace PetHome.Domain.PetEntity;
public class Species
{
    // Для EF core
    private Species() { }

    public Guid Id { get; private set; }
    public SpeciesName Name { get; private set; }
    public IReadOnlyList<Breed> BreadList { get; private set; }
}
