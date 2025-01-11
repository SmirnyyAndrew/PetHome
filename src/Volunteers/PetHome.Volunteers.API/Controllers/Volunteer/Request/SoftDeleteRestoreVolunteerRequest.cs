using PetHome.Volunteers.Application.Features.Write.VolunteerManegment.SoftDeleteRestoreVolunteer;

namespace PetHome.Volunteers.API.Controllers.Volunteer.Request;

public record SoftDeleteRestoreVolunteerRequest(Guid VolunteerId)
{
    public static implicit operator SoftDeleteRestoreVolunteerCommand(SoftDeleteRestoreVolunteerRequest request)
    {
        return new SoftDeleteRestoreVolunteerCommand(request.VolunteerId);
    }
}
