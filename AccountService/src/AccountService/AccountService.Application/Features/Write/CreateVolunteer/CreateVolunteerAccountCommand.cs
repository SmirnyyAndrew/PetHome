using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetHome.SharedKernel.ValueObjects.User;

namespace AccountService.Application.Features.Write.CreateVolunteer;
public record CreateVolunteerAccountCommand(
        string Email,
        string UserName,
        DateTime StartVolunteeringDate,
        IReadOnlyList<RequisitesesDto> Requisites,
        IReadOnlyList<CertificateDto> Certificates) : ICommand;
