using PetHome.Core.Interfaces.FeatureManagment;

namespace PetManagementService.Application.Features.Write.VolunteerManegment.UpdateMainInfoVolunteer;
public record UpdateMainInfoVolunteerCommand(
    Guid Id,
    UpdateMainInfoVolunteerDto UpdateMainInfoDto) : ICommand;
