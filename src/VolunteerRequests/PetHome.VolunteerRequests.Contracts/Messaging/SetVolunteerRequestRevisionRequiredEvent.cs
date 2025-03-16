namespace PetHome.VolunteerRequests.Contracts.Messaging;
public record SetVolunteerRequestRevisionRequiredEvent(
    Guid VolunteerRequestId,
    Guid AdminId,
    Guid UserId,
    Guid DiscussionId,
    string RejectedComment);