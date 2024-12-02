using PetHome.Domain.GeneralValueObjects;

namespace PetHome.Application.Volunteers.CreateVolunteer;

public record RequisitesesDto(string name, string desc, PaymentMethodEnum paymentMethod);

public record CreateVolunteerRequest(
        string firstName,
        string lastName,
        string email,
        string description,
        DateOnly startVolunteeringDate,
        List<string> phoneNumbers,
        List<string> socialNetworks,
        List<RequisitesesDto> requisitesesDto);

