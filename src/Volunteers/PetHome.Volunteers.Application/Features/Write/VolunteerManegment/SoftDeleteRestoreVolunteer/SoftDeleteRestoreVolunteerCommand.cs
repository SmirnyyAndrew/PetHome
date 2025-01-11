using PetHome.Core.Interfaces.FeatureManagment;

namespace PetHome.Volunteers.Application.Features.Write.VolunteerManegment.SoftDeleteRestoreVolunteer;
public record SoftDeleteRestoreVolunteerCommand(Guid VolunteerId) : ICommand;
