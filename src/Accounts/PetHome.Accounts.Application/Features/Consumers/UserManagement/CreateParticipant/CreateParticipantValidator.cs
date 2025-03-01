using FluentValidation;
using PetHome.Accounts.Contracts.Messaging.UserManagment;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Core.ValueObjects.MainInfo;
using PetHome.Core.ValueObjects.User;

namespace PetHome.Accounts.Application.Features.Consumers.UserManagement.CreateParticipant;
public class CreateParticipantValidator : AbstractValidator<CreatedParticipantEvent>
{
    public CreateParticipantValidator()
    {
        RuleFor(a => a.Email).MustBeValueObject(Email.Create);
        RuleFor(a => a.UserName).MustBeValueObject(UserName.Create);
    }
}
