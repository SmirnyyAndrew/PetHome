using FluentValidation;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Core.ValueObjects.PetManagment.Breed;
using PetHome.Core.ValueObjects.PetManagment.Species;

namespace PetHome.Species.Application.Features.Write.CreateSpecies;
public class CreateSpeciesValidator : AbstractValidator<CreateSpeciesCommand>
{
    public CreateSpeciesValidator()
    {
        RuleFor(c => c.SpeciesName).MustBeValueObject(SpeciesName.Create);
    }
}
