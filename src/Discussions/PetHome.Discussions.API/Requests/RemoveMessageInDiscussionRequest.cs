using PetHome.Discussions.Application.Features.Write.RemoveMessageInDiscussion;

namespace PetHome.Discussions.API.Requests;
public record RemoveMessageInDiscussionRequest(Guid UserId, Guid MessageId)
{
    public RemoveMessageInDiscussionCommand ToCommand(Guid DiscussionId)
        => new RemoveMessageInDiscussionCommand(DiscussionId, UserId, MessageId);
}
