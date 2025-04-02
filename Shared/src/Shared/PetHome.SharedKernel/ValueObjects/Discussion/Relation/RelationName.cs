using CSharpFunctionalExtensions;
using PetHome.SharedKernel.Responses.ErrorManagement;

namespace PetHome.SharedKernel.ValueObjects.Discussion.Relation;
public record RelationName
{
    public string Value { get; }
    public static int MinRelationSize = 2;
    public RelationName(string value)
    {
        Value = value;
    }

    public static Result<RelationName, Error> Create(string value)
    {
        if (value.Trim().Count() < MinRelationSize)
            return Errors.Validation("Relation name");
        return new RelationName(value);
    }

    public static implicit operator string(RelationName name) => name.Value;
}
