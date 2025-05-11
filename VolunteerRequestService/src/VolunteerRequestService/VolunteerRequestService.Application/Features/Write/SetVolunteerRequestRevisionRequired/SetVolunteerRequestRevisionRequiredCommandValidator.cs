using FluentValidation;
using PetHome.Core.Application.Validation.Validator;
using PetHome.SharedKernel.ValueObjects.VolunteerRequest;

namespace PetHome.VolunteerRequests.Application.Features.Write.SetVolunteerRequestRevisionRequired;
public class SetVolunteerRequestRevisionRequiredCommandValidator
    : AbstractValidator<SetVolunteerRequestRevisionRequiredCommand>
{
    public SetVolunteerRequestRevisionRequiredCommandValidator()
    {
        RuleFor(r => r.RejectedComment).MustBeValueObject(RequestComment.Create);
    }
}
