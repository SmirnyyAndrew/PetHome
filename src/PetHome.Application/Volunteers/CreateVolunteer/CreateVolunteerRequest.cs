using PetHome.Domain.GeneralValueObjects;

namespace PetHome.Application.Volunteers.CreateVolunteer;

public record CreateVolunteerRequest(Guid id, CreateVolunteerRequestDto CreateVolunteerDto);

public record CreateVolunteerRequestDto(
        string firstName,
        string lastName,
        string email,
        string description,
        DateOnly startVolunteeringDate,
        List<string> phoneNumberList,
        List<string> socialNetworkList,
        List<(string name,string desc, PaymentMethodEnum paymentMethod)> requisitesList);

