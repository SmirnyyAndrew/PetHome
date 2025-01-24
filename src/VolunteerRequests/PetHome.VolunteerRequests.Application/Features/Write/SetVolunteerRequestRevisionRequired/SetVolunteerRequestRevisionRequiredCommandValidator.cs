using FluentValidation;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Core.ValueObjects.VolunteerRequest;

namespace PetHome.VolunteerRequests.Application.Features.Write.SetVolunteerRequestRevisionRequired;
public class SetVolunteerRequestRevisionRequiredCommandValidator
    : AbstractValidator<SetVolunteerRequestRevisionRequiredCommand>
{
    public SetVolunteerRequestRevisionRequiredCommandValidator()
    {
        RuleFor(r => r.RejectedComment).MustBeValueObject(RequestComment.Create);
    }
}
