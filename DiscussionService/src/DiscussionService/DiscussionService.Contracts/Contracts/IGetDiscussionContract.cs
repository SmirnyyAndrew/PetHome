using DiscussionService.Contracts.Dto;

namespace DiscussionService.Contracts.Contracts;
public interface IGetDiscussionContract
{
    public Task<DiscussionDto?> Execute(Guid id, CancellationToken ct);
}
