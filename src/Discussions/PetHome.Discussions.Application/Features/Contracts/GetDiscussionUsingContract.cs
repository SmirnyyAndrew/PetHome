using PetHome.Discussions.Application.Database.Interfaces;
using PetHome.Discussions.Contracts.Contracts;
using PetHome.Discussions.Contracts.Dto;
using PetHome.Discussions.Domain;

namespace PetHome.Discussions.Application.Features.Contracts;
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
