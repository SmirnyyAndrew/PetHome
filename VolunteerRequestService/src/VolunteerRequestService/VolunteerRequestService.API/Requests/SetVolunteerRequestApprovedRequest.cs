using PetHome.Core.ValueObjects.User;
using PetHome.VolunteerRequests.Application.Features.Write.SetVolunteerRequestApproved;

namespace PetHome.VolunteerRequests.API.Requests;
public record SetVolunteerRequestApprovedRequest(
    string Email,
    string UserName,
    DateTime StartVolunteeringDate,
    IReadOnlyList<RequisitesesDto> Requisites,
    IReadOnlyList<CertificateDto> Certificates)
{
    public SetVolunteerRequestApprovedCommand ToCommand(
        Guid volunteerRequestId,Guid adminId)
        => new SetVolunteerRequestApprovedCommand(
                 volunteerRequestId,
                 adminId,
                 Email,
                 UserName,
                 StartVolunteeringDate,
                 Requisites,
                 Certificates);
}
