using PetHome.VolunteerRequests.Application.Features.Write.CreateVolunteerRequest;

namespace PetHome.VolunteerRequests.API.Requests;
public record CreateVolunteerRequestRequest(Guid UserId, string VolunteerInfo)
{
    public static implicit operator CreateVolunteerRequestCommand(CreateVolunteerRequestRequest request)
        => new(request.UserId, request.VolunteerInfo);
}