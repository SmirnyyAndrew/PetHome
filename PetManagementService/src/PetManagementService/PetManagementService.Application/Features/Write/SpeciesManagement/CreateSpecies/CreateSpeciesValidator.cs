using FluentValidation;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Core.ValueObjects.PetManagment.Breed;
using PetHome.Core.ValueObjects.PetManagment.Species;

namespace PetManagementService.Application.Features.Write.SpeciesManagement.CreateSpecies;
public class CreateSpeciesValidator : AbstractValidator<CreateSpeciesCommand>
{
    public CreateSpeciesValidator()
    {
        RuleFor(c => c.SpeciesName).MustBeValueObject(SpeciesName.Create);
    }
}
