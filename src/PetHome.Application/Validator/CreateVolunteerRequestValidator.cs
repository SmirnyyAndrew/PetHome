using FluentValidation;
using PetHome.Application.Volunteers.CreateVolunteer;
using PetHome.Domain.GeneralValueObjects;
using PetHome.Domain.Shared.Error;

namespace PetHome.Application.Validator;
public class CreateVolunteerRequestValidator : AbstractValidator<CreateVolunteerRequest>
{
    public CreateVolunteerRequestValidator()
    {
        RuleFor(c => c.FullNameDto)
            .MustBeValueObject(n => FullName.Create(n.FirstName, n.LastName));

        RuleFor(c => c.Email).MustBeValueObject(Email.Create);

        RuleFor(c => c.Description)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithError(Errors.Validation("Описание"));

        RuleFor(c => c.StartVolunteeringDate).MustBeValueObject(Date.Create);

        RuleForEach(c => c.PhoneNumbers).MustBeValueObject(PhoneNumber.Create);

        RuleForEach(c => c.SocialNetworks).MustBeValueObject(SocialNetwork.Create);

        RuleForEach(c => c.RequisitesesDto).MustBeValueObject(x => Requisites.Create(x.Name, x.Desc, x.PaymentMethod));
    }
}
