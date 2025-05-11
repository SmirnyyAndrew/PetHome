using DiscussionService.Application.Database.Dto;

namespace DiscussionService.Application.Database.Interfaces;
public interface IDiscussionReadDbContext
{
    IQueryable<DiscussionDto> Discussions { get; }
    IQueryable<RelationDto> Relations { get; }
}
