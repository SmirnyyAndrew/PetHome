using PetHome.Core.Interfaces.FeatureManagment;

namespace PetManagementService.Application.Features.Write.VolunteerManegment.SoftDeleteRestoreVolunteer;
public record SoftDeleteRestoreVolunteerCommand(Guid VolunteerId) : ICommand;
