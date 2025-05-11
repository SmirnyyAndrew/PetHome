using CSharpFunctionalExtensions;
using PetHome.SharedKernel.Responses.ErrorManagement;

namespace PetHome.SharedKernel.ValueObjects.User;
public class UserId : ComparableValueObject
{
    public Guid Value { get; }

    private UserId() { }
    private UserId(Guid value)
    {
        Value = value;
    }

    public static Result<UserId, Error> Create(Guid value) => new UserId(value);

    public static Result<UserId, Error> Create() => new UserId(Guid.NewGuid());

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator Guid(UserId id) => id.Value;
}
