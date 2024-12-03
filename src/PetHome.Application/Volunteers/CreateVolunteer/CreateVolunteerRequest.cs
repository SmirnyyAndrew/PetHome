using PetHome.Domain.GeneralValueObjects;

namespace PetHome.Application.Volunteers.CreateVolunteer;

public record RequisitesesDto(string Name, string Desc, PaymentMethodEnum PaymentMethod);

public record CreateVolunteerRequest(
        string FirstName,
        string LastName,
        string Email,
        string Description,
        DateOnly StartVolunteeringDate,
        IEnumerable<string> PhoneNumbers,
        IEnumerable<string> SocialNetworks,
        IEnumerable<RequisitesesDto> RequisitesesDto);

