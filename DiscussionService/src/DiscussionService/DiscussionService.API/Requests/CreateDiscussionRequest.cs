using DiscussionService.Application.Features.Write.CreateDiscussionUsingContract;

namespace DiscussionService.API.Requests;
public record CreateDiscussionRequest(IEnumerable<Guid> UsersIds)
{
    public CreateDiscussionCommand ToCommand(Guid RelationId) =>
        new CreateDiscussionCommand(RelationId, UsersIds);
}
