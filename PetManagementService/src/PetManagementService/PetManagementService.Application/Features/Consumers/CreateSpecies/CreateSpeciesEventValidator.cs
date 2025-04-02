using FluentValidation;
using PetHome.Core.Application.Validation.Validator;
using PetManagementService.Contracts.Messaging.Species;
using PetManagementService.Domain.SpeciesManagment.SpeciesEntity;

namespace PetManagementService.Application.Features.Consumers.CreateSpecies;
public class CreateSpeciesEventValidator : AbstractValidator<CreatedSpeciesEvent>
{
    public CreateSpeciesEventValidator()
    {
        RuleFor(c => c.Name).MustBeValueObject(SpeciesName.Create);
    }
}
