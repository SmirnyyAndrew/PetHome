using PetHome.Core.ValueObjects.User;

namespace AccountService.Contracts.Messaging.UserManagment;
public record CreatedVolunteerAccountEvent(
        Guid UserId,
        string Email,
        string UserName,
        DateTime StartVolunteeringDate,
        IReadOnlyList<RequisitesesDto> Requisites,
        IReadOnlyList<CertificateDto> Certificates);
