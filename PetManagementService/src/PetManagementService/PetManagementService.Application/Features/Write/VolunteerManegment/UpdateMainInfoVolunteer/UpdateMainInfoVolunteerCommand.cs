using PetHome.Core.Application.Interfaces.FeatureManagement;

namespace PetManagementService.Application.Features.Write.VolunteerManegment.UpdateMainInfoVolunteer;
public record UpdateMainInfoVolunteerCommand(
    Guid Id,
    UpdateMainInfoVolunteerDto UpdateMainInfoDto) : ICommand;
