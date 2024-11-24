namespace PetHome.Domain.PetEntity;
public class Breed
{
    // Для EF core
    private Breed() { }
     
    public Guid Id { get; private set; }
    public BreedName Name { get; private set; }  
}
