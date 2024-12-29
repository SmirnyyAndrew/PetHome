using PetHome.Application.Interfaces.FeatureManagment;

namespace PetHome.Application.Features.Write.VolunteerManegment.SoftDeleteRestoreVolunteer;
public record SoftDeleteRestoreVolunteerCommand(Guid VolunteerId) : ICommand;
