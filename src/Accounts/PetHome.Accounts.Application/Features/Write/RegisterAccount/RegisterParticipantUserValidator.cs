using FluentValidation;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Core.ValueObjects.MainInfo;
using PetHome.Core.ValueObjects.User;

namespace PetHome.Accounts.Application.Features.Write.RegisterAccount;
public class RegisterParticipantUserValidator : AbstractValidator<RegisterParticipantUserCommand>
{
    public RegisterParticipantUserValidator()
    {
        RuleFor(r => r.Email).MustBeValueObject(Email.Create);
        RuleFor(r => r.Name).MustBeValueObject(UserName.Create);
        RuleFor(r => r.Password).MustBeValueObject(Password.Create);
    }
}
