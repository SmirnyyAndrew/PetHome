namespace PetHome.Domain.VolunteerEntity;
public record VolunteerId
{  
    public Guid Value { get; }

    private VolunteerId() { }

    private VolunteerId(Guid value)
    {
        Value = value;
    }

    public static VolunteerId Create() => new VolunteerId(Guid.NewGuid());

    public static VolunteerId Create(Guid id) => new VolunteerId(id);

    public static VolunteerId CreateEmpty() => new VolunteerId(Guid.Empty);


    public static implicit operator Guid(VolunteerId volunteerId)
    {
        if (volunteerId == null)
            throw new ArgumentNullException();

        return volunteerId.Value;
    } 
}