namespace PetHome.VolunteerRequests.Contracts.Messaging;
public record CreatedVolunteerRequestEvent(Guid UserId, Guid VolunteerRequestId, string VolunteerInfo);
