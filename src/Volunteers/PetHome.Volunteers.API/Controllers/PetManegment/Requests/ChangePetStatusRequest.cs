using PetHome.Core.ValueObjects.PetManagment.Pet;
using PetHome.Volunteers.Application.Features.Write.PetManegment.ChangePetStatus;

namespace PetHome.Volunteers.API.Controllers.PetManegment.Requests;
public record ChangePetStatusRequest(
    Guid VolunteerId,
    Guid PetId,
    PetStatusEnum NewPetStatus)
{
    public static implicit operator ChangePetStatusCommand(ChangePetStatusRequest request)
    {
        return new ChangePetStatusCommand(
            request.VolunteerId,
            request.PetId,
            request.NewPetStatus);
    }
}
