using PetHome.VolunteerRequests.Application.Features.Write.SetVolunteerRequestRejected;

namespace PetHome.VolunteerRequests.API.Requests;
public record SetVolunteerRequestRejectedRequest(string RejectedComment)
{
    public SetVolunteerRequestRejectedCommand ToCommand(Guid VolunteerRequestId, Guid AdminId)
        => new(VolunteerRequestId, AdminId, RejectedComment);
}
