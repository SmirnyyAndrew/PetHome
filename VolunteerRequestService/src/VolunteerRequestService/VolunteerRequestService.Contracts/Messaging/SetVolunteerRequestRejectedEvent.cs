namespace PetHome.VolunteerRequests.Contracts.Messaging;
public record SetVolunteerRequestRejectedEvent(
    Guid VolunteerRequestId,
    Guid AdminId,
    Guid UserId,
    Guid DiscussionId,
    string RejectedComment);