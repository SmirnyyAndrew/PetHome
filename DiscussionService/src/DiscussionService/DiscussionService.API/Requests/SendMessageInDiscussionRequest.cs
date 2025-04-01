using DiscussionService.Application.Features.Write.SendMessageInDiscussion;

namespace DiscussionService.API.Requests;
public record SendMessageInDiscussionRequest(Guid UserId, string Message)
{
    public SendMessageInDiscussionCommand ToCommand(Guid DiscussionId)
        => new SendMessageInDiscussionCommand(DiscussionId, UserId, Message);

}
