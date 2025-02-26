namespace PetHome.Discussions.Contracts.Messaging;
public record CreatedDiscussionEvent(IEnumerable<Guid> UsersIds, Guid RelationId = default);