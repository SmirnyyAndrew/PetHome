using PetHome.Volunteers.Application.Features.Write.VolunteerManegment.HardDeleteVolunteer;
using PetHome.Volunteers.Domain.PetManagment.VolunteerEntity;

namespace PetHome.Volunteers.API.Controllers.Volunteer.Request;

public record HardDeleteVolunteerRequest(VolunteerId VolunteerId)
{
    public static implicit operator HardDeleteVolunteerCommand(HardDeleteVolunteerRequest request)
    {
        return new HardDeleteVolunteerCommand(request.VolunteerId);
    }
}