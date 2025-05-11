using FluentValidation;
using PetHome.Core.Application.Validation.Validator;
using PetHome.SharedKernel.ValueObjects.MainInfo;
using PetHome.SharedKernel.ValueObjects.User;

namespace AccountService.Application.Features.Write.Registration.RegisterUser;
public class RegisterUserValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserValidator()
    {
        RuleFor(r => r.Email).MustBeValueObject(Email.Create);
        RuleFor(r => r.UserName).MustBeValueObject(UserName.Create);
        RuleFor(r => r.Password).MustBeValueObject(Password.Create);
    }
}
