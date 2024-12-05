using PetHome.Domain.PetManagment.GeneralValueObjects;

namespace PetHome.Application.Features.Volunteers.VolunteerDtos;
public record RequisitesesDto(string Name, string Desc, PaymentMethodEnum PaymentMethod);
