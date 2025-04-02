using CSharpFunctionalExtensions;
using PetHome.SharedKernel.Responses.ErrorManagement;

namespace PetHome.SharedKernel.ValueObjects.Discussion;
public class DiscussionId : ComparableValueObject
{
    public Guid Value { get; }
    public DiscussionId(Guid value)
    {
        Value = value;
    }

    public static Result<DiscussionId, Error> Create(Guid value) => new DiscussionId(value);

    public static Result<DiscussionId, Error> Create() => new DiscussionId(Guid.NewGuid());

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    { 
        yield return Value;
    }

    public static implicit operator Guid(DiscussionId id) => id.Value;
}
