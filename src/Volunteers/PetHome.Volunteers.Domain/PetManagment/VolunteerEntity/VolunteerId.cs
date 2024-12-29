using CSharpFunctionalExtensions;
using PetHome.Domain.Shared.Error;

namespace PetHome.Volunteers.Domain.PetManagment.VolunteerEntity;
public record VolunteerId
{
    public Guid Value { get; }

    private VolunteerId() { }

    private VolunteerId(Guid value)
    {
        Value = value;
    }

    public static Result<VolunteerId, Error> Create() => new VolunteerId(Guid.NewGuid());

    public static Result<VolunteerId, Error> Create(Guid id) => new VolunteerId(id);

    public static Result<VolunteerId, Error> CreateEmpty() => new VolunteerId(Guid.Empty);

    public static implicit operator Guid(VolunteerId volunteerId)
    {
        if (volunteerId == null)
            throw new ArgumentNullException();

        return volunteerId.Value;
    }
}