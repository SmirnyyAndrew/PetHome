using PetHome.Core.Interfaces.FeatureManagment;

namespace PetHome.Volunteers.Application.Features.Write.VolunteerManegment.UpdateMainInfoVolunteer;
public record UpdateMainInfoVolunteerCommand(
    Guid Id,
    UpdateMainInfoVolunteerDto UpdateMainInfoDto) : ICommand;
