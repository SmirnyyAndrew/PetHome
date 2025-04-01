using PetHome.Core.ValueObjects.MainInfo;
using PetHome.Core.ValueObjects.User;
using PetManagementService.Application.Features.Write.VolunteerManegment.CreateVolunteer;

namespace PetManagementService.API.Controllers.VolunteerEntity.Request;

public record CreateVolunteerRequest(
        FullNameDto FullNameDto,
        string Email,
        string Description,
        string UserName,
        DateTime StartVolunteeringDate,
        IEnumerable<string> PhoneNumbers,
        IEnumerable<SocialNetworkDto> SocialNetworks,
        IEnumerable<CertificateDto> Certificates,
        IEnumerable<RequisitesesDto> RequisitesesDto)
{
    public static implicit operator CreateVolunteerCommand(CreateVolunteerRequest request)
    {
        return new CreateVolunteerCommand(
            request.FullNameDto,
            request.Email,
            request.Description,
            request.UserName,
            request.StartVolunteeringDate,
            request.PhoneNumbers,
            request.SocialNetworks,
            request.Certificates,
            request.RequisitesesDto);
    }
}

