using PetManagementService.Application.Features.Write.PetManegment.SoftDeleteRestore;

namespace PetManagementService.API.Controllers.PetManagment.Requests;
public record SoftDeleteRestorePetRequest(
    Guid VolunteerId,
    Guid PetId,
    bool ToDelete)
{
    public static implicit operator SoftDeleteRestorePetCommand(SoftDeleteRestorePetRequest request)
    {
        return new SoftDeleteRestorePetCommand(
            request.VolunteerId,
            request.PetId,
            request.ToDelete);
    }
}
