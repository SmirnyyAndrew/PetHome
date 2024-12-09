using PetHome.Domain.PetManagment.GeneralValueObjects;

namespace PetHome.Application.Features.Dtos.Volunteer;
public record RequisitesesDto(string Name, string Desc, PaymentMethodEnum PaymentMethod);
