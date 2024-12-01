namespace PetHome.Domain.PetEntity;
public record PetShelterId
{
    public Guid Value { get; }

    private PetShelterId() { } 
    private PetShelterId(Guid value)
    {
        Value = value;
    }

    public static PetShelterId Create() => new PetShelterId(Guid.NewGuid()); 

    public static  PetShelterId Create(Guid id) => new PetShelterId(id); 

    public static PetShelterId CreateEmpty() => new PetShelterId(Guid.Empty); 

    public static implicit operator Guid(PetShelterId petShelterId)
    {
        if (petShelterId == null)
            throw new ArgumentNullException();

        return petShelterId.Value;
    }
}