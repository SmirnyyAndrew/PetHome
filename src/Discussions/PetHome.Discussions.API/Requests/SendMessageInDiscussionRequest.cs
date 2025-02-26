using PetHome.Discussions.Application.Features.Write.SendMessageInDiscussion;

namespace PetHome.Discussions.API.Requests;
public record SendMessageInDiscussionRequest(Guid UserId, string Message)
{
    public SendMessageInDiscussionCommand ToCommand(Guid DiscussionId)
        => new SendMessageInDiscussionCommand(DiscussionId, UserId, Message);

}
