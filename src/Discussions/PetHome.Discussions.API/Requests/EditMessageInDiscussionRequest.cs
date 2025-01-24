using PetHome.Discussions.Application.Features.Write.EditMessageInDiscussion;

namespace PetHome.Discussions.API.Requests;
public record EditMessageInDiscussionRequest(
    Guid DiscussionId,
    Guid UserId,
    Guid MessageId,
    string NewMessageText)
{
    public static implicit operator EditMessageInDiscussionCommand(EditMessageInDiscussionRequest request)
    {
        return new EditMessageInDiscussionCommand(
            request.DiscussionId,
            request.UserId,
            request.MessageId,
            request.NewMessageText);
    }
}
