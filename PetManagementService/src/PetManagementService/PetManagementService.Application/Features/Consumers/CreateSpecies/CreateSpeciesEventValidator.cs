using FluentValidation;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Core.ValueObjects.PetManagment.Species;
using PetManagementService.Contracts.Messaging.Species;

namespace PetManagementService.Application.Features.Consumers.CreateSpecies;
public class CreateSpeciesEventValidator : AbstractValidator<CreatedSpeciesEvent>
{
    public CreateSpeciesEventValidator()
    {
        RuleFor(c => c.Name).MustBeValueObject(SpeciesName.Create);
    }
}
