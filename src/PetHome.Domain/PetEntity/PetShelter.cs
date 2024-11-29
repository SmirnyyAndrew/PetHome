namespace PetHome.Domain.PetEntity;
public class PetShelter
{
    // Для EF core
    public PetShelter() { }

    public PetShelterId Id { get; private set; }
    public ShelterName Name { get; private set; }
}
