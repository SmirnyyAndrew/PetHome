using PetHome.VolunteerRequests.Application.Features.Read.GetAllUserVolunteerRequests;

namespace PetHome.VolunteerRequests.API.Requests;
public record GetAllUserVolunteerRequestsRequest(Guid UserId, int PageSize, int PageNum)
{
    public static implicit operator GetAllUserVolunteerRequestsQuery(GetAllUserVolunteerRequestsRequest request)
        => new(request.UserId, request.PageSize, request.PageNum);
}
