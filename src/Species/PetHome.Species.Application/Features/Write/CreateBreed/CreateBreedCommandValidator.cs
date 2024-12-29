using FluentValidation;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Species.Domain.SpeciesManagment.BreedEntity;
using PetHome.Species.Domain.SpeciesManagment.SpeciesEntity;

namespace PetHome.Species.Application.Features.Write.CreateBreed;
public class CreateBreedCommandValidator : AbstractValidator<CreateBreedCommand>
{
    public CreateBreedCommandValidator()
    {
        RuleForEach(v => v.Breeds)
            .MustBeValueObject(b => Breed.Create(b, SpeciesId.CreateEmpty().Value));
    }
}
