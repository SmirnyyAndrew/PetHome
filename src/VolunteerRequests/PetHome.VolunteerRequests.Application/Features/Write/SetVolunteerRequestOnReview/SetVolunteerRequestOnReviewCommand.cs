using PetHome.Core.Interfaces.FeatureManagment;

namespace PetHome.VolunteerRequests.Application.Features.Write.SetVolunteerRequestOnReview;
public record SetVolunteerRequestOnReviewCommand(
    Guid VolunteerRequestId,
    Guid AdminId,
    Guid UserId,
    Guid DiscussionId,
    string RelationName) : ICommand;