using PetHome.VolunteerRequests.Application.Features.Read.GetAllUserVolunteerRequests;

namespace PetHome.VolunteerRequests.API.Requests;
public record GetAllUserVolunteerRequestsRequest(int PageSize, int PageNum)
{
    public GetAllUserVolunteerRequestsQuery ToQuery(Guid userId)
        => new(userId, PageSize, PageNum);
}
