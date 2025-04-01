using PetHome.Core.Interfaces.FeatureManagment;

namespace PetManagementService.Application.Features.Write.PetManegment.HardDelete;
public record HardDeletePetCommand(
    Guid VolunteerId,
    Guid PetId) : ICommand;
