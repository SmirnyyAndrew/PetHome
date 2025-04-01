namespace DiscussionService.Application.Database.Dto;
public class DiscussionDto
{
    public Guid Id { get; private set; }
    public Guid RelationId { get; private set; }
    public RelationDto Relation { get; private set; } 
    public ICollection<MessageDto> Messages { get; private set; } = [];
    public string Status { get; private set; }

    private DiscussionDto() { }
}
