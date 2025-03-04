using FluentValidation;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Core.ValueObjects.MainInfo;
using PetHome.Core.ValueObjects.PetManagment.Extra;
using PetHome.Core.ValueObjects.User;

namespace PetHome.Accounts.Application.Features.Write.CreateVolunteer;
public class CreateVolunteerAccountCommandValidator : AbstractValidator<CreateVolunteerAccountCommand>
{
    public CreateVolunteerAccountCommandValidator()
    {
        RuleFor(v => v.Email).MustBeValueObject(Email.Create);
        RuleFor(v => v.UserName).MustBeValueObject(UserName.Create);
        RuleFor(v => v.StartVolunteeringDate).MustBeValueObject(Date.Create);
       
        RuleForEach(v => v.Requisites)
            .MustBeValueObject(r => Requisites.Create(r.Name, r.Desc, PaymentMethodEnum.Cash));
        
        RuleForEach(v => v.Certificates)
            .MustBeValueObject(r => Certificate.Create(r.Name,r.Value)); 
    }
}
