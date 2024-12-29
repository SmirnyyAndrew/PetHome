using PetHome.Application.Features.Write.VolunteerManegment.HardDeleteVolunteer;
using PetHome.Domain.PetManagment.VolunteerEntity;

namespace PetHome.API.Controllers.Volunteer.Request;

public record HardDeleteVolunteerRequest(VolunteerId VolunteerId)
{
    public static implicit operator HardDeleteVolunteerCommand(HardDeleteVolunteerRequest request)
    {
        return new HardDeleteVolunteerCommand(request.VolunteerId);
    }
}