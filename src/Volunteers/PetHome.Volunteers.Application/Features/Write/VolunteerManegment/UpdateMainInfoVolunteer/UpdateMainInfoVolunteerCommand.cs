using PetHome.Application.Interfaces.FeatureManagment;

namespace PetHome.Application.Features.Write.VolunteerManegment.UpdateMainInfoVolunteer;
public record UpdateMainInfoVolunteerCommand(
    Guid Id,
    UpdateMainInfoVolunteerDto UpdateMainInfoDto) : ICommand;
