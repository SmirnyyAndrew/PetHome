using FluentValidation;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Core.ValueObjects.PetManagment.Species;
using PetHome.Species.Domain.SpeciesManagment.BreedEntity;

namespace PetHome.Species.Application.Features.Write.CreateBreed;
public class CreateBreedCommandValidator : AbstractValidator<CreateBreedCommand>
{
    public CreateBreedCommandValidator()
    {
        RuleForEach(v => v.Breeds)
            .MustBeValueObject(b => Breed.Create(b, SpeciesId.CreateEmpty().Value));
    }
}
