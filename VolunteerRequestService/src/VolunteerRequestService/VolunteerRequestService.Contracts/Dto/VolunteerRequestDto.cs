namespace PetHome.VolunteerRequests.Contracts.Dto;
public record VolunteerRequestDto(
    Guid Id, 
    Guid? AdminId, 
    Guid UserId, 
    string VolunteerInfo,
    string Status,
    DateTime CreatedAt,
    string? RejectedComment,
    Guid? DiscussionId); 