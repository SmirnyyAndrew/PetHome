using DiscussionService.Application.Features.Write.OpenDiscussion;

namespace DiscussionService.API.Requests;
public record OpenDiscussionRequest(Guid DiscussionId)
{
    public static implicit operator OpenDiscussionCommand(OpenDiscussionRequest request)
        => new OpenDiscussionCommand(request.DiscussionId);
};
