using FluentValidation;
using PetHome.Accounts.Contracts.Messaging.UserManagment;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Core.ValueObjects.MainInfo;
using PetHome.Core.ValueObjects.User;

namespace PetHome.Accounts.Application.Features.Consumers.UserManagement.CreateUser;
public class CreatedUserEventValidator : AbstractValidator<CreatedUserEvent>
{
    public CreatedUserEventValidator()
    {
        RuleFor(u => u.Email).MustBeValueObject(Email.Create);
        RuleFor(u => u.UserName).MustBeValueObject(UserName.Create);
    }
}
