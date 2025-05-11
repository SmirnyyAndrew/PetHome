using DiscussionService.Application.Features.Read.GetAllDiscussionByRelationId;

namespace DiscussionService.API.Requests;
public record GetAllDiscussionByRelationIdRequest(int PageSize, int PageNum)
{
    public GetAllDiscussionByRelationIdQuery ToCommand(Guid relationId)
        => new GetAllDiscussionByRelationIdQuery(
            relationId,
            PageSize,
            PageNum);
}
