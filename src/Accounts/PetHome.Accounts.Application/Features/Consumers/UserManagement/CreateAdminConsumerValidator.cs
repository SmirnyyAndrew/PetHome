using FluentValidation;
using PetHome.Accounts.Contracts.Messaging.UserManagment;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Core.ValueObjects.MainInfo;
using PetHome.Core.ValueObjects.User;

namespace PetHome.Accounts.Application.Features.Consumers.UserManagement;
public class CreateAdminConsumerValidator : AbstractValidator<CreatedAdminEvent>
{
    public CreateAdminConsumerValidator()
    {
        RuleFor(a => a.Email).MustBeValueObject(Email.Create);
        RuleFor(a => a.UserName).MustBeValueObject(UserName.Create);
    }
}
