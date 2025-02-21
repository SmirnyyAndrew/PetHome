using CSharpFunctionalExtensions;
using PetHome.Core.Response.ErrorManagment;

namespace PetHome.Core.ValueObjects.Discussion.Relation;
public record RelationId : IComparable<RelationId>
{
    public Guid Value { get; }
    public RelationId(Guid value)
    {
        Value = value;
    }

    public static Result<RelationId, Error> Create(Guid value) => new RelationId(value);

    public static Result<RelationId, Error> Create() => new RelationId(Guid.NewGuid());

    public int CompareTo(RelationId? other) => 0;

    public static implicit operator Guid(RelationId id) => id.Value;
}
