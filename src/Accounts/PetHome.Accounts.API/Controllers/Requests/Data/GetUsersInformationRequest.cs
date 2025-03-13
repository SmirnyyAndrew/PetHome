using PetHome.Accounts.Application.Features.Read.GetUsersInformation;
using PetHome.Core.Enums;
using PetHome.Core.Response.Dto;

namespace PetHome.Accounts.API.Controllers.Requests.Data;
public record GetUsersInformationRequest(PagedListDto PaginationSettings, UserFilter FilterType, string Filter)
{
    public static implicit operator GetUsersInformationQuery(GetUsersInformationRequest request)
     => new(request.PaginationSettings, request.FilterType, request.Filter);
}
