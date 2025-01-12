using PetHome.Core.ValueObjects.Discussion.Relation;

namespace PetHome.Discussions.Domain;
public class Relation
{
    public RelationId Id { get; private set; }
    public RelationName Name { get; private set; }
}
