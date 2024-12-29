using PetHome.Volunteers.Application.Features.Read.VolunteerManegment.GetAllVolunteersWithPagination;

namespace PetHome.Volunteers.API.Controllers.Volunteer.Request;

public record GetAllVolunteersWithPaginationRequest(int PageSize, int PageNum)
{
    public static implicit operator GetAllVolunteersWithPaginationQuery(
        GetAllVolunteersWithPaginationRequest command)
    {
        return new GetAllVolunteersWithPaginationQuery(command.PageSize, command.PageNum);
    }
}
