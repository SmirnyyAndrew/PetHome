using PetHome.Core.Models;
using PetHome.Core.ValueObjects.Discussion.Relation;

namespace PetHome.Discussions.Domain;
public class Relation : DomainEntity<RelationId>
{
    public RelationId Id { get; private set; }
    public RelationName Name { get; private set; }
    public IReadOnlyList<Discussion> Discussions { get; private set; } = [];

    private Relation(RelationId id) : base(id) { }
    private Relation(RelationName name) : base(RelationId.Create().Value)
    {
        Name = name;
    }

    public static Relation Create(RelationName name) => new Relation(name);
}
