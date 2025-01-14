using PetHome.Discussions.Application.Features.Write.RemoveMessageInDiscussion;

namespace PetHome.Discussions.API.Requests;
public record RemoveMessageInDiscussionRequest(Guid DiscussionId, Guid UserId, Guid MessageId)
{ 
    public static implicit operator RemoveMessageInDiscussionCommand(RemoveMessageInDiscussionRequest request)
    {
        return new RemoveMessageInDiscussionCommand(request.DiscussionId, request.UserId, request.MessageId);
    }
}
