using PetHome.Domain.GeneralValueObjects;

namespace PetHome.Application.Volunteers.CreateVolunteer;

public record RequisitesesDto(string Name, string Desc, PaymentMethodEnum PaymentMethod);

public record CreateVolunteerRequest(
        string FirstName,
        string LastName,
        string Email,
        string Description,
        DateOnly StartVolunteeringDate,
        List<string> PhoneNumbers,
        List<string> SocialNetworks,
        List<RequisitesesDto> RequisitesesDto);

