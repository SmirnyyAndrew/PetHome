using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.ValueObjects.User;

namespace PetHome.VolunteerRequests.Application.Features.Write.SetVolunteerRequestApproved;
public record SetVolunteerRequestApprovedCommand(
    Guid VolunteerRequestId,
    Guid AdminId,
    string Email,
    string UserName,
    DateTime StartVolunteeringDate,
    IReadOnlyList<RequisitesesDto> Requisites,
    IReadOnlyList<CertificateDto> Certificates) : ICommand;
