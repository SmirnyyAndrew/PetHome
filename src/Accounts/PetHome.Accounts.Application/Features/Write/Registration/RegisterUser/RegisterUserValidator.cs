using FluentValidation;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Core.ValueObjects.MainInfo;
using PetHome.Core.ValueObjects.User;

namespace PetHome.Accounts.Application.Features.Write.Registration.RegisterAccount;
public class RegisterUserValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserValidator()
    {
        RuleFor(r => r.Email).MustBeValueObject(Email.Create);
        RuleFor(r => r.UserName).MustBeValueObject(UserName.Create);
        RuleFor(r => r.Password).MustBeValueObject(Password.Create);
    }
}
