using PetHome.Core.Interfaces.FeatureManagment;

namespace PetHome.Volunteers.Contracts.CreateVolunteerContract;

public record CreateVolunteerCommand(
        FullNameDto FullNameDto,
        string Email,
        string Description,
        DateTime StartVolunteeringDate,
        IEnumerable<string> PhoneNumbers,
        IEnumerable<string> SocialNetworks,
        IEnumerable<RequisitesesDto> RequisitesesDto) : ICommand;

