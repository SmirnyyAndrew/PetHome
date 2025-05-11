using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetHome.SharedKernel.ValueObjects.MainInfo;
using PetHome.SharedKernel.ValueObjects.User;

namespace PetManagementService.Application.Features.Write.VolunteerManegment.CreateVolunteer;
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
