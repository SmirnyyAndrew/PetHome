namespace PetHome.Discussions.Contracts.CreateDiscussion;
public record CreateDiscussionCommand(Guid RelationId, IEnumerable<Guid> UsersIds);