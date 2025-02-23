using PetHome.Discussions.Contracts.CreateDiscussion;

namespace PetHome.Discussions.API.Requests;
public record CreateDiscussionRequest(IEnumerable<Guid> UsersIds)
{
    public CreateDiscussionCommand ToCommand(Guid RelationId) =>
        new CreateDiscussionCommand(RelationId, UsersIds);
}
