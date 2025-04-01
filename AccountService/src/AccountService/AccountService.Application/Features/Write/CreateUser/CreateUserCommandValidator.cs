using FluentValidation;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Core.ValueObjects.MainInfo;
using PetHome.Core.ValueObjects.User;

namespace AccountService.Application.Features.Write.CreateUser;
public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(u => u.Email).MustBeValueObject(Email.Create);
        RuleFor(u => u.UserName).MustBeValueObject(UserName.Create);
    }
}
