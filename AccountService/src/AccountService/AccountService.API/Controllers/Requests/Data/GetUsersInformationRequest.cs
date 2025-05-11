using AccountService.Application.Features.Read.GetUsersInformation;
using PetHome.Core.Domain.Models;
using PetHome.SharedKernel.Responses.Dto;

namespace AccountService.API.Controllers.Requests.Data;
public record GetUsersInformationRequest(PagedListDto PaginationSettings, UserFilterDto UserFilter)
{
    public static implicit operator GetUsersInformationQuery(GetUsersInformationRequest request)
     => new(request.PaginationSettings, request.UserFilter);
}
