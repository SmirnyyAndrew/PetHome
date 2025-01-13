using PetHome.VolunteerRequests.Application.Features.Write.SetVolunteerRequestApproved;

namespace PetHome.VolunteerRequests.API.Requests;
public record SetVolunteerRequestApprovedRequest(Guid VolunteerRequestId, Guid AdminId)
{
    public static implicit operator SetVolunteerRequestApprovedCommand(SetVolunteerRequestApprovedRequest request)
        => new(request.VolunteerRequestId, request.AdminId);
}
