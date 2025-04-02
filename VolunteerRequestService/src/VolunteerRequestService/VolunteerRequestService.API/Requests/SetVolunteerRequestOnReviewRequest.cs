using PetHome.VolunteerRequests.Application.Features.Write.SetVolunteerRequestOnReview;

namespace PetHome.VolunteerRequests.API.Requests;
public record SetVolunteerRequestOnReviewRequest(  
Guid AdminId,
string RelationName)
{
    public SetVolunteerRequestOnReviewCommand ToCommand(Guid VolunteerRequestId, Guid DiscussionId, Guid UserId)
    {
        return new SetVolunteerRequestOnReviewCommand(
            VolunteerRequestId,  
            AdminId,
            UserId,
            DiscussionId, 
            RelationName);
    }
}