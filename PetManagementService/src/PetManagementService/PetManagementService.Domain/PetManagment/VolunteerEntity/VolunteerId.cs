using CSharpFunctionalExtensions;
using PetHome.SharedKernel.Responses.ErrorManagement;

namespace PetManagementService.Domain.PetManagment.VolunteerEntity;
public class VolunteerId : ComparableValueObject
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

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator Guid(VolunteerId id) => id.Value;
}