using PetHome.VolunteerRequests.Application.Features.Write.SetVolunteerRequestRejected;

namespace PetHome.VolunteerRequests.API.Requests;
public record SetVolunteerRequestRejectedRequest(
    Guid VolunteerRequestId,
    Guid AdminId,
    string RejectedComment)
{

    public static implicit operator SetVolunteerRequestRejectedCommand(SetVolunteerRequestRejectedRequest request)
        => new(request.VolunteerRequestId, request.AdminId, request.RejectedComment);
}
