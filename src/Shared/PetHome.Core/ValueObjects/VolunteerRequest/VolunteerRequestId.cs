using CSharpFunctionalExtensions;
using PetHome.Core.Response.ErrorManagment;

namespace PetHome.Core.ValueObjects.VolunteerRequest;
public record VolunteerRequestId : IComparable<VolunteerRequestId>
{
    public Guid Value { get; }
    public VolunteerRequestId(Guid value)
    {
        Value = value;
    }

    public static Result<VolunteerRequestId, Error> Create(Guid value)
    {
        return new VolunteerRequestId(value);
    }
    public static Result<VolunteerRequestId, Error> Create()
    {
        return new VolunteerRequestId(Guid.NewGuid());
    }

    public int CompareTo(VolunteerRequestId? other) => 0;

    public static implicit operator Guid(VolunteerRequestId id) => id.Value;
}
