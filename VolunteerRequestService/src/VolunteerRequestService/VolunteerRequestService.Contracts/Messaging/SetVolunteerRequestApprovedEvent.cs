namespace PetHome.VolunteerRequests.Contracts.Messaging;
public record SetVolunteerRequestApprovedEvent(
    Guid UserId, 
    Guid AdminId,
    string UserName,
    Guid VolunteerRequestId,
    string VolunteerInfo,
    DateTime CreatedAt);