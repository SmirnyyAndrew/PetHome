using FluentValidation;
using PetHome.Core.Application.Validation.Validator;
using PetHome.SharedKernel.ValueObjects.MainInfo;
using PetHome.SharedKernel.ValueObjects.User;

namespace AccountService.Application.Features.Write.CreateParticipant;
public class CreateParticipantCommandValidator : AbstractValidator<CreateParticipantCommand>
{
    public CreateParticipantCommandValidator()
    {
        RuleFor(a => a.Email).MustBeValueObject(Email.Create);
        RuleFor(a => a.UserName).MustBeValueObject(UserName.Create);
    }
}
