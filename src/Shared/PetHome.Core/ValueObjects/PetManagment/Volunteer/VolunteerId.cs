using CSharpFunctionalExtensions;
using PetHome.Core.Response.ErrorManagment;

namespace PetHome.Core.ValueObjects.PetManagment.Volunteer;
public record VolunteerId : IComparable<VolunteerId>
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

    public int CompareTo(VolunteerId? other) => 0;

    public static implicit operator Guid(VolunteerId volunteerId)
    {
        if (volunteerId == null)
            throw new ArgumentNullException();

        return volunteerId.Value;
    }
}