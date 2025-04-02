using FluentValidation;
using PetHome.Core.Application.Validation.Validator;
using PetHome.SharedKernel.ValueObjects.MainInfo;
using PetHome.SharedKernel.ValueObjects.User;

namespace AccountService.Application.Features.Write.CreateUser;
public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(u => u.Email).MustBeValueObject(Email.Create);
        RuleFor(u => u.UserName).MustBeValueObject(UserName.Create);
    }
}
