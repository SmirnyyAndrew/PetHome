namespace PetHome.Discussions.Contracts.Messaging;
public record OpenedDiscussionEvent(
    Guid DiscussionId,
    Guid RelationId,
    string RelationName,
    IEnumerable<Guid> UsersIds);