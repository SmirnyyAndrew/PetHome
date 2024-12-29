using PetHome.Application.Interfaces.FeatureManagment;

namespace PetHome.Application.Features.Write.PetManegment.HardDelete;
public record HardDeletePetCommand(
    Guid VolunteerId,
    Guid PetId) : ICommand;
