using PetHome.Discussions.Application.Features.Write.CreateDiscussionUsingContract;
using PetHome.Discussions.Contracts.Messaging;

namespace PetHome.Discussions.API.Requests;
public record CreateDiscussionRequest(IEnumerable<Guid> UsersIds)
{
    public CreateDiscussionCommand ToCommand(Guid RelationId) =>
        new CreateDiscussionCommand(RelationId, UsersIds);
}
