using PetHome.Core.Application.Interfaces.FeatureManagement;

namespace DiscussionService.Application.Features.Read.GetAllDiscussionByRelationId;
public record GetAllDiscussionByRelationIdQuery(Guid RelationId, int PageSize, int PageNum) : IQuery;
