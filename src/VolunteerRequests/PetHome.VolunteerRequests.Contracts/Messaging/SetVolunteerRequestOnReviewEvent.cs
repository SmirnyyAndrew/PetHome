namespace PetHome.VolunteerRequests.Contracts.Messaging;
public record SetVolunteerRequestOnReviewEvent(
    Guid VolunteerRequestId,
    Guid AdminId,
    Guid UserId,
    Guid DiscussionId,
    string RelationName);