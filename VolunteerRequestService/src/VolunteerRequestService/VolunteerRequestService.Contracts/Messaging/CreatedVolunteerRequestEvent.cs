namespace PetHome.VolunteerRequests.Contracts.Messaging;
public record CreatedVolunteerRequestEvent(
    Guid VolunteerRequestId, 
    Guid UserId,  
    string VolunteerInfo,
    DateTime CreatedAt);
