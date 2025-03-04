using FluentValidation;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Core.ValueObjects.MainInfo;
using PetHome.Core.ValueObjects.User;

namespace PetHome.Accounts.Application.Features.Write.CreateAdmin;
public class CreateAdminCommandValidator : AbstractValidator<CreateAdminCommand>
{
    public CreateAdminCommandValidator()
    {
        RuleFor(a => a.Email).MustBeValueObject(Email.Create);
        RuleFor(a => a.UserName).MustBeValueObject(UserName.Create);
    }
}
