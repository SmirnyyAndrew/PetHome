using PetHome.Discussions.Application.Features.Write.OpenDiscussion;

namespace PetHome.Discussions.API.Requests;
public record OpenDiscussionRequest(Guid DiscussionId)
{
    public static implicit operator OpenDiscussionCommand(OpenDiscussionRequest request)
        => new OpenDiscussionCommand(request.DiscussionId);
};
