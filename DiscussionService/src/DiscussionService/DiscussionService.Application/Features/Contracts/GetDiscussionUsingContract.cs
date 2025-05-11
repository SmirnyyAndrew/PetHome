using DiscussionService.Application.Database.Interfaces;
using DiscussionService.Contracts.Contracts;
using DiscussionService.Contracts.Dto;
using PetHome.Discussions.Domain;

namespace DiscussionService.Application.Features.Contracts;
public class GetDiscussionUsingContract(IDiscussionRepository repository) 
    : IGetDiscussionContract
{
    public async Task<DiscussionDto?> Execute(Guid id, CancellationToken ct)
    {
        Discussion? discussion = await repository.GetDiscussionById(id, ct);   
        if(discussion is null) 
            return null;

        DiscussionDto discussionDto = new DiscussionDto(
            discussion.Id,
            discussion.RelationId,
            discussion.Relation?.Name,
            discussion.UserIds.Select(u => u.Value),
            discussion.Status.ToString());

        return discussionDto;
    }
}
