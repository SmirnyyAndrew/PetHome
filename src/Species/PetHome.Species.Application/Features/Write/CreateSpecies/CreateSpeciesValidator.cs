using FluentValidation;
using PetHome.Application.Validator;
using PetHome.Domain.PetManagment.PetEntity;

namespace PetHome.Species.Application.Features.Write.CreateSpecies;
public class CreateSpeciesValidator : AbstractValidator<CreateSpeciesCommand>
{
    public CreateSpeciesValidator()
    {
        RuleFor(c => c.SpeciesName).MustBeValueObject(BreedName.Create);
    }
}
