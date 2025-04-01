namespace DiscussionService.Application.Database.Dto;
public class RelationDto
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public ICollection<DiscussionDto> Discussions { get; private set; } = [];

    private RelationDto() { }
}
