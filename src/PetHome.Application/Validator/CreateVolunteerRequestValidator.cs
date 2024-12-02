using FluentValidation;
using PetHome.Application.Volunteers.CreateVolunteer;
using PetHome.Domain.GeneralValueObjects;

namespace PetHome.Application.Validator;
public class CreateVolunteerRequestValidator : AbstractValidator<CreateVolunteerRequest>
{
    public CreateVolunteerRequestValidator()
    {
        RuleFor(c => new { c.firstName, c.lastName })
            .MustBeValueObject(n => FullName.Create(n.firstName, n.lastName));

        RuleFor(c => c.email).MustBeValueObject(Email.Create);

        RuleFor(c => c.description).Must(x => !string.IsNullOrWhiteSpace(x));

        RuleFor(c=>c.startVolunteeringDate).MustBeValueObject(Date.Create);

        RuleForEach(c => c.phoneNumbers).MustBeValueObject(PhoneNumber.Create);

        RuleForEach(c=>c.socialNetworks).MustBeValueObject(SocialNetwork.Create);

        RuleForEach(c => c.requisitesesDto).MustBeValueObject(x => Requisites.Create(x.name, x.desc, x.paymentMethod));
    }
}
