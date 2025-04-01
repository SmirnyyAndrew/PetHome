using PetHome.VolunteerRequests.Application.Features.Read.GetAllSubmittedVolunteerRequests;

namespace PetHome.VolunteerRequests.API.Requests;
public record GetAllSubmittedVolunteerRequestsRequest(int PageSize, int PageNum)
{
    public static implicit operator GetAllSubmittedVolunteerRequestsQuery(GetAllSubmittedVolunteerRequestsRequest request)
        => new(request.PageSize, request.PageNum);
}
