using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetHome.SharedKernel.ValueObjects.User;

namespace PetHome.VolunteerRequests.Application.Features.Write.SetVolunteerRequestApproved;
public record SetVolunteerRequestApprovedCommand(
    Guid VolunteerRequestId,
    Guid AdminId,
    string Email,
    string UserName,
    DateTime StartVolunteeringDate,
    IReadOnlyList<RequisitesesDto> Requisites,
    IReadOnlyList<CertificateDto> Certificates) : ICommand;
