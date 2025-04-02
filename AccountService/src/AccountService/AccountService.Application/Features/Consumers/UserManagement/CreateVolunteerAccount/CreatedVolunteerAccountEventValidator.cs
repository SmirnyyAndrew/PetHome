using AccountService.Contracts.Messaging.UserManagement;
using FluentValidation;
using PetHome.Core.Application.Validation.Validator;
using PetHome.SharedKernel.ValueObjects.MainInfo;
using PetHome.SharedKernel.ValueObjects.PetManagment.Extra;
using PetHome.SharedKernel.ValueObjects.User;

namespace AccountService.Application.Features.Consumers.UserManagement.CreateVolunteerAccount;
public class CreatedVolunteerAccountEventValidator : AbstractValidator<CreatedVolunteerAccountEvent>
{
    public CreatedVolunteerAccountEventValidator()
    {
        RuleFor(v => v.Email).MustBeValueObject(Email.Create);
        RuleFor(v => v.UserName).MustBeValueObject(UserName.Create);
        RuleFor(v => v.StartVolunteeringDate).MustBeValueObject(Date.Create);

        RuleForEach(v => v.Requisites)
            .MustBeValueObject(r => Requisites.Create(r.Name, r.Desc, PaymentMethodEnum.Cash));

        RuleForEach(v => v.Certificates)
            .MustBeValueObject(r => Certificate.Create(r.Name, r.Value));
    }
}
