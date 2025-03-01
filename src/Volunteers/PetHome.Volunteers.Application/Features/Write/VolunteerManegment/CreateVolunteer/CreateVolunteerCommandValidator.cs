using FluentValidation;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Core.ValueObjects.MainInfo;
using PetHome.Core.ValueObjects.PetManagment.Extra;
using PetHome.Core.ValueObjects.User;

namespace PetHome.Volunteers.Application.Features.Write.VolunteerManegment.CreateVolunteer;
public class CreateVolunteerCommandValidator : AbstractValidator<CreateVolunteerCommand>
{
    public CreateVolunteerCommandValidator()
    {
        RuleFor(c => c.FullNameDto)
            .MustBeValueObject(n => FullName.Create(n.FirstName, n.LastName));

        RuleFor(c => c.Email).MustBeValueObject(Email.Create);

        RuleFor(c => c.Description).MustBeValueObject(Description.Create);

        RuleFor(c => c.UserName).MustBeValueObject(UserName.Create);

        RuleFor(c => c.StartVolunteeringDate).MustBeValueObject(Date.Create);

        RuleForEach(c => c.PhoneNumbers).MustBeValueObject(PhoneNumber.Create);

        RuleForEach(c => c.SocialNetworks).MustBeValueObject(s=> SocialNetwork.Create(s.url));

        RuleForEach(c => c.Requisiteses)
            .MustBeValueObject(x => Requisites.Create(x.Name, x.Desc, x.PaymentMethod));
    }
}
