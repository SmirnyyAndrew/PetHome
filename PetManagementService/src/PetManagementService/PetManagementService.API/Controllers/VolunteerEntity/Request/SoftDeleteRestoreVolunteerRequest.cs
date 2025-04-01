using PetManagementService.Application.Features.Write.VolunteerManegment.SoftDeleteRestoreVolunteer;

namespace PetManagementService.API.Controllers.VolunteerEntity.Request;

public record SoftDeleteRestoreVolunteerRequest(Guid VolunteerId)
{
    public static implicit operator SoftDeleteRestoreVolunteerCommand(SoftDeleteRestoreVolunteerRequest request)
    {
        return new SoftDeleteRestoreVolunteerCommand(request.VolunteerId);
    }
}
