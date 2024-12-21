using PetHome.Domain.PetManagment.PetEntity;

namespace PetHome.Application.Features.Write.PetManegment.ChangePetStatus;
public record ChangePetStatusCommand(
    Guid VolunteerId,
    Guid PetId,
    PetStatusEnum NewPetStatus);
