using FluentValidation;
using PetHome.Application.Features.Volunteers.PetManegment.CreateSpeciesVolunteer;
using PetHome.Application.Validator;
using PetHome.Domain.PetManagment.PetEntity;

namespace PetHome.Application.Features.Volunteers.PetManegment.CreateSpecies;
public class CreateSpeciesValidator: AbstractValidator<CreateSpeciesRequest>
{
    public CreateSpeciesValidator()
    {
        RuleFor(c => c.BreedName).MustBeValueObject(BreedName.Create);
    }
}
