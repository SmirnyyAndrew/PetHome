using PetHome.VolunteerRequests.Application.Features.Write.SetVolunteerRequestRevisionRequired;

namespace PetHome.VolunteerRequests.API.Requests;
public record SetVolunteerRequestRevisionRequiredRequest(
    string RejectedComment)
{
    public SetVolunteerRequestRevisionRequiredCommand ToCommand(Guid VolunteerRequestId, Guid AdminId)
        => new(VolunteerRequestId, AdminId, RejectedComment);
}
