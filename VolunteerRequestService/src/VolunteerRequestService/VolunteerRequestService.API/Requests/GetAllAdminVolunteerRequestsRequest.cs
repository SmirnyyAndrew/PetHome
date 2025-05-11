using PetHome.VolunteerRequests.Application.Features.Read.GetAllAdminVolunteerRequests;
using PetHome.VolunteerRequests.Domain;

namespace PetHome.VolunteerRequests.API.Requests;
public record GetAllAdminVolunteerRequestsRequest( 
    int PageSize, 
    int PageNum,
    VolunteerRequestStatus? status)
{
    public GetAllAdminVolunteerRequestsQuery ToQuery(Guid adminId)
        => new(adminId, PageSize, PageNum, status);
}
