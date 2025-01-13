using FluentValidation;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Core.ValueObjects.VolunteerRequest;

namespace PetHome.VolunteerRequests.Application.Features.Write.CreateVolunteerRequests;
public class CreateVolunteerRequestsCommandValidator
    : AbstractValidator<CreateVolunteerRequestsCommand>
{
    public CreateVolunteerRequestsCommandValidator()
    {
        RuleFor(r => r.VolunteerInfo).MustBeValueObject(VolunteerInfo.Create);
    }
}
