using PetHome.VolunteerRequests.Application.Features.Write.SetVolunteerRequestRevisionRequired;

namespace PetHome.VolunteerRequests.API.Requests;
public record SetVolunteerRequestRevisionRequiredRequest(
    Guid VolunteerRequestId,
    Guid AdminId,
    string RejectedComment)
{
    public static implicit operator SetVolunteerRequestRevisionRequiredCommand(SetVolunteerRequestRevisionRequiredRequest request)
        => new(request.VolunteerRequestId, request.AdminId, request.RejectedComment);
}
