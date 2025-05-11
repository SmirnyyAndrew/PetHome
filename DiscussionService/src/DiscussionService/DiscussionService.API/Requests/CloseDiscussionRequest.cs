using DiscussionService.Application.Features.Write.CloseDiscussion;

namespace DiscussionService.API.Requests;
public record CloseDiscussionRequest(Guid DiscussionId)
{
    public static implicit operator CloseDiscussionCommand(CloseDiscussionRequest request)
    {
        return new CloseDiscussionCommand(request.DiscussionId);
    }
}
