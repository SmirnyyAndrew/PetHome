namespace PetHome.Domain.PetEntity;
public class PetShelter
{
    // Для EF core
    private PetShelter() { }

    public PetShelterId Id { get; private set; }
    public ShelterName Name { get; private set; }
}
