namespace DiscussionService.Application.Database.Dto;
public class MessageDto
{
    public Guid Id { get; private set; }
    public string? Text { get; private set; }
    public Guid UserId { get; private set; } 
    public Guid DiscussionId { get; private set; }
    public DiscussionDto Discussion { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool IsEdited { get; private set; } = false;

    private MessageDto() { } 
}
