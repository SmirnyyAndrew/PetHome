using FluentValidation;
using PetHome.Application.Validator;
using PetHome.Domain.PetManagment.GeneralValueObjects;

namespace PetHome.Application.Features.Volunteers.VolunteerManegment.UpdateMainInfoVolunteer;
public class UpdateMainInfoVolunteerRequestValidator
    : AbstractValidator<UpdateMainInfoVolunteerRequest>
{
    public UpdateMainInfoVolunteerRequestValidator()
    {
        RuleFor(u => u.UpdateMainInfoDto.FullNameDto).MustBeValueObject(n => FullName.Create(n.FirstName, n.LastName));

        RuleFor(u => u.UpdateMainInfoDto.Description).MustBeValueObject(Description.Create);

        RuleForEach(u => u.UpdateMainInfoDto.PhoneNumbers)
            .MustBeValueObject(PhoneNumber.Create);

        RuleFor(u => u.UpdateMainInfoDto.Email).MustBeValueObject(Email.Create);
    }
}
