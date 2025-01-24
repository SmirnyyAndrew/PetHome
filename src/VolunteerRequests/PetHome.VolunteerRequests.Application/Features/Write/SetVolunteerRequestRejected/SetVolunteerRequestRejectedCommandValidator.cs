using FluentValidation;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Core.ValueObjects.VolunteerRequest;

namespace PetHome.VolunteerRequests.Application.Features.Write.SetVolunteerRequestRejected;
public class SetVolunteerRequestRejectedCommandValidator
    :AbstractValidator<SetVolunteerRequestRejectedCommand>
{
    public SetVolunteerRequestRejectedCommandValidator()
    {
        RuleFor(r => r.RejectedComment).MustBeValueObject(RequestComment.Create);
    }
}
