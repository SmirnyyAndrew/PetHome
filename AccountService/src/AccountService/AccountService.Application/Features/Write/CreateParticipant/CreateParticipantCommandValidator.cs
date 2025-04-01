using FluentValidation;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Core.ValueObjects.MainInfo;
using PetHome.Core.ValueObjects.User;

namespace AccountService.Application.Features.Write.CreateParticipant;
public class CreateParticipantCommandValidator : AbstractValidator<CreateParticipantCommand>
{
    public CreateParticipantCommandValidator()
    {
        RuleFor(a => a.Email).MustBeValueObject(Email.Create);
        RuleFor(a => a.UserName).MustBeValueObject(UserName.Create);
    }
}
