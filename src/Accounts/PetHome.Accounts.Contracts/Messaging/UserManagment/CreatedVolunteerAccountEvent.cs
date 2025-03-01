using PetHome.Core.ValueObjects.User;

namespace PetHome.Accounts.Contracts.Messaging.UserManagment;
public record CreatedVolunteerAccountEvent(
        string Email,
        string UserName,
        DateTime StartVolunteeringDate,
        IReadOnlyList<RequisitesesDto> Requisites,
        IReadOnlyList<CertificateDto> Certificates,
        Guid Id = default);
