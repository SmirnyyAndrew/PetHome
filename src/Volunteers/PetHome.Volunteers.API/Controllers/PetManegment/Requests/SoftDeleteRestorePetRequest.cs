using PetHome.Application.Features.Write.PetManegment.SoftDeleteRestore;

namespace PetHome.API.Controllers.PetManegment.Requests;
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
