using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.ValueObjects.PetManagment.Pet;

namespace PetHome.Volunteers.Application.Features.Write.PetManegment.ChangePetStatus;
public record ChangePetStatusCommand(
    Guid VolunteerId,
    Guid PetId,
    PetStatusEnum NewPetStatus) : ICommand;
