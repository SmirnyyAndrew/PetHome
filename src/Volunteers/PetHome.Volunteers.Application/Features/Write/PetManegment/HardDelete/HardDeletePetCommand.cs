using PetHome.Core.Interfaces.FeatureManagment;

namespace PetHome.Volunteers.Application.Features.Write.PetManegment.HardDelete;
public record HardDeletePetCommand(
    Guid VolunteerId,
    Guid PetId) : ICommand;
