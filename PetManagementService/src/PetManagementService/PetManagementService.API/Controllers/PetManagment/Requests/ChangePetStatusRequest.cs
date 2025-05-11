using PetManagementService.Domain.PetManagment.PetEntity;
using PetManagementService.Application.Features.Write.PetManegment.ChangePetStatus;

namespace PetManagementService.API.Controllers.PetManagment.Requests;
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
