using FluentValidation;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Species.Domain.SpeciesManagment.BreedEntity;

namespace PetHome.Species.Application.Features.Write.CreateSpecies;
public class CreateSpeciesValidator : AbstractValidator<CreateSpeciesCommand>
{
    public CreateSpeciesValidator()
    {
        RuleFor(c => c.SpeciesName).MustBeValueObject(BreedName.Create);
    }
}
