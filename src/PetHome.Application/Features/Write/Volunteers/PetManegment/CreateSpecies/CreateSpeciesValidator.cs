using FluentValidation;
using PetHome.Application.Validator;
using PetHome.Domain.PetManagment.PetEntity;

namespace PetHome.Application.Features.Write.Volunteers.PetManegment.CreateSpecies;
public class CreateSpeciesValidator : AbstractValidator<CreateSpeciesCommand>
{
    public CreateSpeciesValidator()
    {
        RuleFor(c => c.SpeciesName).MustBeValueObject(BreedName.Create);
    }
}
