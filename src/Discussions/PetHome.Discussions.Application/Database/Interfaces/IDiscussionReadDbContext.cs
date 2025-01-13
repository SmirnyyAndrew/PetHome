using PetHome.Discussions.Application.Database.Dto;

namespace PetHome.Discussions.Application.Database.Interfaces;
public interface IDiscussionReadDbContext
{
    IQueryable<DiscussionDto> Discussions { get; }
    IQueryable<RelationDto> Relations { get; }
}
