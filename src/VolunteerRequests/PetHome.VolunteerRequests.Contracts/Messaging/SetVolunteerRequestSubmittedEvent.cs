namespace PetHome.VolunteerRequests.Contracts.Messaging;
public record SetVolunteerRequestSubmittedEvent(
            Guid Id,
            Guid AdminId,
            Guid UserId,
            Guid DiscussionId);