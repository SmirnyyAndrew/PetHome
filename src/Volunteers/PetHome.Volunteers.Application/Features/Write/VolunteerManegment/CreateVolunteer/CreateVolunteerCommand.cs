using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.ValueObjects.MainInfo;
using PetHome.Core.ValueObjects.User;

namespace PetHome.Volunteers.Application.Features.Write.VolunteerManegment.CreateVolunteer;
public record CreateVolunteerCommand(
        FullNameDto FullNameDto,
        string Email,
        string Description,
        string UserName,
        DateTime StartVolunteeringDate,
        IEnumerable<string> PhoneNumbers,
        IEnumerable<SocialNetworkDto> SocialNetworks,
        IEnumerable<CertificateDto> Certificates,
        IEnumerable<RequisitesesDto> Requisiteses) : ICommand;
