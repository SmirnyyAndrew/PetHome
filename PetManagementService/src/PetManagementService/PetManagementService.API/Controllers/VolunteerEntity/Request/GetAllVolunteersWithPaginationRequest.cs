using PetManagementService.Application.Features.Read.VolunteerManegment.GetAllVolunteersWithPagination;

namespace PetManagementService.API.Controllers.VolunteerEntity.Request;

public record GetAllVolunteersWithPaginationRequest(int PageSize, int PageNum)
{
    public static implicit operator GetAllVolunteersWithPaginationQuery(
        GetAllVolunteersWithPaginationRequest command)
    {
        return new GetAllVolunteersWithPaginationQuery(command.PageSize, command.PageNum);
    }
}
