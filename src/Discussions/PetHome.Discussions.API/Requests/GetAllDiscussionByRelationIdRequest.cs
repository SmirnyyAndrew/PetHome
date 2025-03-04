using PetHome.Discussions.Application.Features.Read.GetAllDiscussionByRelationId;

namespace PetHome.Discussions.API.Requests;
public record GetAllDiscussionByRelationIdRequest(int PageSize, int PageNum)
{
    public GetAllDiscussionByRelationIdQuery ToCommand(Guid relationId)
        => new GetAllDiscussionByRelationIdQuery(
            relationId,
            PageSize,
            PageNum);
}
