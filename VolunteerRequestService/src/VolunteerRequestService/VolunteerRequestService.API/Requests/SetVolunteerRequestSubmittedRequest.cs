using PetHome.VolunteerRequests.Application.Features.Write.SetVolunteerRequestSubmitted;

namespace PetHome.VolunteerRequests.API.Requests;
public record SetVolunteerRequestSubmittedRequest(Guid VolunteerRequestId, Guid AdminId)
{
    public static implicit operator SetVolunteerRequestSubmittedCommand(SetVolunteerRequestSubmittedRequest request)
        => new(request.VolunteerRequestId, request.AdminId);
}
