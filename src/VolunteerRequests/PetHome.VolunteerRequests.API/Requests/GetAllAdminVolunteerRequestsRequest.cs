using PetHome.VolunteerRequests.Application.Features.Read.GetAllAdminVolunteerRequests;

namespace PetHome.VolunteerRequests.API.Requests;
public record GetAllAdminVolunteerRequestsRequest(Guid AdminId, int PageSize, int PageNum)
{
    public static implicit operator GetAllAdminVolunteerRequestsQuery(GetAllAdminVolunteerRequestsRequest request)
        => new(request.AdminId, request.PageSize, request.PageNum);
}
