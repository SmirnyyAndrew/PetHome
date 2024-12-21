using PetHome.Application.Features.Write.PetManegment.HardDelete;

namespace PetHome.API.Controllers.PetManegment.Requests;
public record HardDeleteRequest(
    Guid VolunteerId,
    Guid PetId)
{
    public static implicit operator HardDeleteCommand(HardDeleteRequest request)
    {
        return new HardDeleteCommand(request.VolunteerId, request.PetId);
    }
}
