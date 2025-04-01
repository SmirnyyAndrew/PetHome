using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.ValueObjects.PetManagment.Pet;

namespace PetManagementService.Application.Features.Write.PetManegment.ChangePetStatus;
public record ChangePetStatusCommand(
    Guid VolunteerId,
    Guid PetId,
    PetStatusEnum NewPetStatus) : ICommand;
