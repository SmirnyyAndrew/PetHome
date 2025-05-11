using FluentValidation;
using PetHome.Core.Application.Validation.Validator;
using PetHome.SharedKernel.ValueObjects.VolunteerRequest;
using PetHome.VolunteerRequests.Contracts.Messaging;

namespace PetHome.VolunteerRequests.Application.Features.Consumers;
public class CreatedVolunteerRequestEventValidator
    : AbstractValidator<CreatedVolunteerRequestEvent>
{
    public CreatedVolunteerRequestEventValidator()
    {
        RuleFor(r => r.VolunteerInfo).MustBeValueObject(VolunteerInfo.Create);
    }
}
