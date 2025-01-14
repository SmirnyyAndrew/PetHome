using PetHome.Discussions.Application.Features.Write.SendMessageInDiscussion;

namespace PetHome.Discussions.API.Requests;
public record SendMessageInDiscussionRequest(Guid DiscussionId, Guid UserId, string Message)
{ 
    public static implicit operator SendMessageInDiscussionCommand(SendMessageInDiscussionRequest request)
    {
        return new SendMessageInDiscussionCommand(request.DiscussionId, request.UserId, request.Message);
    }
}
