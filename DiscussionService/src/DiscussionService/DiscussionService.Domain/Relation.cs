using PetHome.Core.Domain.Models;
using PetHome.SharedKernel.ValueObjects.Discussion.Relation;

namespace PetHome.Discussions.Domain;
public class Relation : DomainEntity<RelationId>
{
    public RelationId Id { get; private set; }
    public RelationName Name { get; private set; }
    public IReadOnlyList<Discussion> Discussions { get; private set; } = [];

    private Relation(RelationId id, RelationName name) : base(id)
    {
        Id = id;
        Name = name;
    }

    public static Relation Create(RelationName name)
    {
        Relation relation = new Relation(
            RelationId.Create().Value,
            name);
        return relation;
    }
}
