using PetHome.Discussions.Application.Features.Read.GetAllDiscussionByRelationId;

namespace PetHome.Discussions.API.Requests;
public record GetAllDiscussionByRelationIdRequest(Guid RelationId, int PageSize, int PageNum)
{
    public static implicit operator GetAllDiscussionByRelationIdQuery(GetAllDiscussionByRelationIdRequest request)
    {
        return new GetAllDiscussionByRelationIdQuery(
            request.RelationId,
            request.PageSize,
            request.PageNum);
    }
}
