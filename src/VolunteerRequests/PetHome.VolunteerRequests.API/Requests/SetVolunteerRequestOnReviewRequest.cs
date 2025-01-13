using PetHome.VolunteerRequests.Application.Features.Write.SetVolunteerRequestOnReview;

namespace PetHome.VolunteerRequests.API.Requests;
public record SetVolunteerRequestOnReviewRequest(
    Guid VolunteerRequestId,
    Guid AdminId,
    Guid DiscussionId)
{
    public static implicit operator SetVolunteerRequestOnReviewCommand(SetVolunteerRequestOnReviewRequest request)
        => new(request.VolunteerRequestId, request.AdminId, request.DiscussionId);
}