using PetHome.Core.ValueObjects.Discussion.Relation;

namespace PetHome.Discussions.Domain;
public class Relation
{
    public RelationId Id { get; private set; }
    public RelationName Name { get; private set; }
    public IReadOnlyList<Discussion> Discussions { get; private set; } = [];

    private Relation() { }
    private Relation(RelationName name)
    {
        Id = RelationId.Create().Value;
        Name = name;
    }

    public static Relation Create(RelationName name) => new Relation(name);
}
