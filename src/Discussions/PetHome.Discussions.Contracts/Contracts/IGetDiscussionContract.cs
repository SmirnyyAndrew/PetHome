using PetHome.Discussions.Contracts.Dto;

namespace PetHome.Discussions.Contracts.Contracts;
public interface IGetDiscussionContract
{
    public Task<DiscussionDto?> Execute(Guid id, CancellationToken ct);
}
