using PetHome.SharedKernel.ValueObjects.User;

namespace AccountService.Contracts.Messaging.UserManagement;
public record CreatedVolunteerAccountEvent(
        Guid UserId,
        string Email,
        string UserName,
        DateTime StartVolunteeringDate,
        IReadOnlyList<RequisitesesDto> Requisites,
        IReadOnlyList<CertificateDto> Certificates);
