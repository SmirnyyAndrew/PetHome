using FluentValidation;
using PetHome.Application.Validator;
using PetHome.Domain.PetManagment.PetEntity;

namespace PetHome.Application.Features.Volunteers.PetManegment.CreateBreedVolunteer;
public class CreateBreedValidator : AbstractValidator<CreateBreedCommand>
{
    public CreateBreedValidator()
    {
        RuleForEach(v => v.Breeds)
            .MustBeValueObject(b => Breed.Create(b, SpeciesId.CreateEmpty().Value));
    }
}
