namespace DiscussionService.Contracts.Messaging;
public record ClosedDiscussionEvent(
    Guid DiscussionId, 
    Guid RelationId, 
    string RelationName, 
    IEnumerable<Guid> UsersIds); 