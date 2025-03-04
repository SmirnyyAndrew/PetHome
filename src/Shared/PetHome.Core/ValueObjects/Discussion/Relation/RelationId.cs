using CSharpFunctionalExtensions;
using PetHome.Core.Response.ErrorManagment;

namespace PetHome.Core.ValueObjects.Discussion.Relation;
public class RelationId : ComparableValueObject
{
    public Guid Value { get; }
    public RelationId(Guid value)
    {
        Value = value;
    }

    public static Result<RelationId, Error> Create(Guid value) => new RelationId(value);

    public static Result<RelationId, Error> Create() => new RelationId(Guid.NewGuid());

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator Guid(RelationId id) => id.Value;
}
