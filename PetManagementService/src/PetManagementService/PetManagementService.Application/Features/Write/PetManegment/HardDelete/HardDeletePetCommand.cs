using PetHome.Core.Application.Interfaces.FeatureManagement;

namespace PetManagementService.Application.Features.Write.PetManegment.HardDelete;
public record HardDeletePetCommand(
    Guid VolunteerId,
    Guid PetId) : ICommand;
