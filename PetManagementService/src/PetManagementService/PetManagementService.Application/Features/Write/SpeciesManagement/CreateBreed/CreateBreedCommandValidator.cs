using FluentValidation;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Core.ValueObjects.PetManagment.Species;
using PetManagementService.Domain.SpeciesManagment.BreedEntity;

namespace PetManagementService.Application.Features.Write.SpeciesManagement.CreateBreed;
public class CreateBreedCommandValidator : AbstractValidator<CreateBreedCommand>
{
    public CreateBreedCommandValidator()
    {
        RuleForEach(v => v.Breeds)
            .MustBeValueObject(b => Breed.Create(b, SpeciesId.CreateEmpty().Value));
    }
}
