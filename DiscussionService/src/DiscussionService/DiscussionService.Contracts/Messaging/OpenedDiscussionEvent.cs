namespace DiscussionService.Contracts.Messaging;
public record OpenedDiscussionEvent(
    Guid DiscussionId,
    Guid RelationId,
    string RelationName,
    IEnumerable<Guid> UsersIds);