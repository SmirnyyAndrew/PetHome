using FluentValidation;
using PetHome.Core.Application.Validation.Validator;
using PetHome.SharedKernel.ValueObjects.MainInfo;
using PetHome.SharedKernel.ValueObjects.PetManagment.Extra;
using PetManagementService.Contracts.Messaging.Volunteer;

namespace PetManagementService.Application.Features.Consumers.CreateVolunteer;
public class CreateVolunteerEventValidator : AbstractValidator<CreatedVolunteerEvent>
{
    public CreateVolunteerEventValidator()
    {
        RuleFor(c => c.FullNameDto)
            .MustBeValueObject(n => FullName.Create(n.FirstName, n.LastName));

        RuleFor(c => c.Email).MustBeValueObject(Email.Create);

        RuleFor(c => c.Description).MustBeValueObject(Description.Create);

        RuleFor(c => c.StartVolunteeringDate).MustBeValueObject(Date.Create);

        RuleForEach(c => c.PhoneNumbers).MustBeValueObject(PhoneNumber.Create);

        RuleForEach(c => c.SocialNetworks).MustBeValueObject(SocialNetwork.Create);

        RuleForEach(c => c.RequisitesesDto)
            .MustBeValueObject(x => Requisites.Create(x.Name, x.Desc, x.PaymentMethod));
    }
}
