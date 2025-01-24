using FluentValidation;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Core.ValueObjects.VolunteerRequest;

namespace PetHome.VolunteerRequests.Application.Features.Write.CreateVolunteerRequest;
public class CreateVolunteerRequestCommandValidator
    : AbstractValidator<CreateVolunteerRequestCommand>
{
    public CreateVolunteerRequestCommandValidator()
    {
        RuleFor(r => r.VolunteerInfo).MustBeValueObject(VolunteerInfo.Create);
    }
}
