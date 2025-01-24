using PetHome.Discussions.Application.Features.Write.CloseDiscussion;

namespace PetHome.Discussions.API.Requests;
public record CloseDiscussionRequest(Guid DiscussionId)
{
    public static implicit operator CloseDiscussionCommand(CloseDiscussionRequest request)
    {
        return new CloseDiscussionCommand(request.DiscussionId);
    }
}
