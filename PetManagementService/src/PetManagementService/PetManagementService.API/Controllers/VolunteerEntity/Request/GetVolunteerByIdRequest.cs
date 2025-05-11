using PetManagementService.Application.Features.Read.VolunteerManegment.GetVolunteerById;

namespace PetManagementService.API.Controllers.VolunteerEntity.Request;

public record GetVolunteerByIdRequest(Guid VolunteerId)
{
    public static implicit operator GetVolunteerByIdQuery(GetVolunteerByIdRequest request)
    {
        return new GetVolunteerByIdQuery(request.VolunteerId);
    }
}