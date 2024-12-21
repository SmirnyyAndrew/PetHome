using PetHome.Application.Features.Write.PetManegment.SoftDelete;

namespace PetHome.API.Controllers.PetManegment.Requests;
public record SoftDeleteRestoreRequest(
    Guid VolunteerId,
    Guid PetId,
    bool ToDelete)
{
    public static implicit operator SoftDeleteRestoreCommand(SoftDeleteRestoreRequest request)
    {
        return new SoftDeleteRestoreCommand(
            request.VolunteerId,
            request.PetId,
            request.ToDelete);
    }
}
