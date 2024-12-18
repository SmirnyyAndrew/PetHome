using PetHome.Application.Features.Read.VolunteerManegment.GetAllVolunteersWithPagination;

namespace PetHome.API.Controllers.Volunteer.Request;

public record GetAllVolunteersWithPaginationCommand(int PageSize, int PageNum)
{
    public static implicit operator GetAllVolunteersWithPaginationQuery(
        GetAllVolunteersWithPaginationCommand command)
    {
        return new GetAllVolunteersWithPaginationQuery(command.PageSize, command.PageNum);
    }
}
