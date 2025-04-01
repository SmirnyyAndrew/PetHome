using DiscussionService.Application.Features.Write.RemoveMessageInDiscussion;

namespace DiscussionService.API.Requests;
public record RemoveMessageInDiscussionRequest(Guid UserId, Guid MessageId)
{
    public RemoveMessageInDiscussionCommand ToCommand(Guid DiscussionId)
        => new RemoveMessageInDiscussionCommand(DiscussionId, UserId, MessageId);
}
