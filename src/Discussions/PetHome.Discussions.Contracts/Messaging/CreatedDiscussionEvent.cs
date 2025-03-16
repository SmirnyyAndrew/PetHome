namespace PetHome.Discussions.Contracts.Messaging;
public record CreatedDiscussionEvent(
    Guid DiscussionId,
    Guid RelationId,
    string RelationName,
    IEnumerable<Guid> UsersIds);