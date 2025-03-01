using FluentValidation;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Core.ValueObjects.VolunteerRequest;
using PetHome.VolunteerRequests.Contracts.Messaging;

namespace PetHome.VolunteerRequests.Application.Features.Write.CreateVolunteerRequest;
public class CreatedVolunteerRequestEventValidator
    : AbstractValidator<CreatedVolunteerRequestEvent>
{
    public CreatedVolunteerRequestEventValidator()
    {
        RuleFor(r => r.VolunteerInfo).MustBeValueObject(VolunteerInfo.Create);
    }
}
