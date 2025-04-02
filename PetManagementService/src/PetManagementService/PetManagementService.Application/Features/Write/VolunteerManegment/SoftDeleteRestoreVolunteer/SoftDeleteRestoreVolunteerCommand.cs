using PetHome.Core.Application.Interfaces.FeatureManagement;

namespace PetManagementService.Application.Features.Write.VolunteerManegment.SoftDeleteRestoreVolunteer;
public record SoftDeleteRestoreVolunteerCommand(Guid VolunteerId) : ICommand;
