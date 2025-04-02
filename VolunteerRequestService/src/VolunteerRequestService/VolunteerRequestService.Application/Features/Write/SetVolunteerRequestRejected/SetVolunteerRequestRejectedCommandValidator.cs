using FluentValidation;
using PetHome.Core.Application.Validation.Validator;
using PetHome.SharedKernel.ValueObjects.VolunteerRequest;

namespace PetHome.VolunteerRequests.Application.Features.Write.SetVolunteerRequestRejected;
public class SetVolunteerRequestRejectedCommandValidator
    :AbstractValidator<SetVolunteerRequestRejectedCommand>
{
    public SetVolunteerRequestRejectedCommandValidator()
    {
        RuleFor(r => r.RejectedComment).MustBeValueObject(RequestComment.Create);
    }
}
