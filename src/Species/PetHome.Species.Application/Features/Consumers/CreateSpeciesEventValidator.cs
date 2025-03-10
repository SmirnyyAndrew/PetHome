using FluentValidation;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Core.ValueObjects.PetManagment.Species;
using PetHome.Species.Contracts.Messaging;

namespace PetHome.Species.Application.Features.Consumers;
public class CreateSpeciesEventValidator : AbstractValidator<CreatedSpeciesEvent>
{
    public CreateSpeciesEventValidator()
    {
        RuleFor(c => c.Name).MustBeValueObject(SpeciesName.Create);
    }
}
