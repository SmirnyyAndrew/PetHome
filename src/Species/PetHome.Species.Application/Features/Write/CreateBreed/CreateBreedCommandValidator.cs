using FluentValidation;
using PetHome.Application.Validator;
using PetHome.Domain.PetManagment.PetEntity;

namespace PetHome.Species.Application.Features.Write.CreateBreed;
public class CreateBreedCommandValidator : AbstractValidator<CreateBreedCommand>
{
    public CreateBreedCommandValidator()
    {
        RuleForEach(v => v.Breeds)
            .MustBeValueObject(b => Breed.Create(b, SpeciesId.CreateEmpty().Value));
    }
}
