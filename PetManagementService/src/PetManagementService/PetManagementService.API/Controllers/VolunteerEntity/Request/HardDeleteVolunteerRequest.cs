using PetManagementService.Domain.PetManagment.VolunteerEntity;
using PetManagementService.Application.Features.Write.VolunteerManegment.HardDeleteVolunteer;

namespace PetManagementService.API.Controllers.VolunteerEntity.Request;

public record HardDeleteVolunteerRequest(VolunteerId VolunteerId)
{
    public static implicit operator HardDeleteVolunteerCommand(HardDeleteVolunteerRequest request)
    {
        return new HardDeleteVolunteerCommand(request.VolunteerId);
    }
}