using PetHome.Core.Interfaces.FeatureManagment;

namespace PetHome.Volunteers.Application.Features.Write.PetManegment.SoftDeleteRestore;
public record SoftDeleteRestorePetCommand(
    Guid VolunteerId,
    Guid PetId,
    bool ToDelete) : ICommand;
