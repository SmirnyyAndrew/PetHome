using PetHome.VolunteerRequests.Application.Features.Read.GetAllAdminVolunteerRequests;
using PetHome.VolunteerRequests.Domain;

namespace PetHome.VolunteerRequests.API.Requests;
public record GetAllAdminVolunteerRequestsRequest(
    Guid AdminId, 
    int PageSize, 
    int PageNum,
    VolunteerRequestStatus? status)
{
    public static implicit operator GetAllAdminVolunteerRequestsQuery(GetAllAdminVolunteerRequestsRequest request)
        => new(request.AdminId, request.PageSize, request.PageNum, request.status);
}
