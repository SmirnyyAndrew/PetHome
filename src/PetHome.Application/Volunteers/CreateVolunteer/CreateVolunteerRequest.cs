using PetHome.Domain.GeneralValueObjects;

namespace PetHome.Application.Volunteers.CreateVolunteer;

public record CreateVolunteerRequest(CreateVolunteerRequestDto CreateVolunteerDto);

public record CreateVolunteerRequestDto(
        string firstName,
        string lastName,
        string email,
        string description,
        DateOnly startVolunteeringDate,
        List<string> phoneNumbers,
        List<string> socialNetworks,
        List<(string name,string desc, PaymentMethodEnum paymentMethod)>? requisiteses);

