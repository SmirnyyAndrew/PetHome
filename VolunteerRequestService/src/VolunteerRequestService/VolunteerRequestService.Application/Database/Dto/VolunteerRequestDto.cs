namespace PetHome.VolunteerRequests.Application.Database.Dto;
public class VolunteerRequestDto
{
    public Guid Id { get; private set; }
    public Guid? AdminId { get; private set; }
    public Guid? UserId { get; private set; }
    public string? VolunteerInfo { get; private set; }
    public string? Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public string? RejectedComment { get; private set; }
    public Guid? DiscussionId { get; private set; }

}
