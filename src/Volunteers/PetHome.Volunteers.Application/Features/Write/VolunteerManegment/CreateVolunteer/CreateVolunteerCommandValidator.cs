using FluentValidation;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Core.ValueObjects;

namespace PetHome.Volunteers.Application.Features.Write.VolunteerManegment.CreateVolunteer;
public class CreateVolunteerCommandValidator : AbstractValidator<CreateVolunteerCommand>
{
    public CreateVolunteerCommandValidator()
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
