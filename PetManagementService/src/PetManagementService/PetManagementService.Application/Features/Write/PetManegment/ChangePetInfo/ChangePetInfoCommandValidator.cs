using FluentValidation;
using PetHome.Core.Application.Validation.Validator;
using PetHome.SharedKernel.ValueObjects.MainInfo;
using PetHome.SharedKernel.ValueObjects.PetManagment.Extra;
using PetManagementService.Domain.PetManagment.PetEntity;
using PetManagementService.Domain.PetManagment.VolunteerEntity;
using PetManagementService.Domain.SpeciesManagment.BreedEntity;
using PetManagementService.Domain.SpeciesManagment.SpeciesEntity;

namespace PetManagementService.Application.Features.Write.PetManegment.ChangePetInfo;
public class ChangePetInfoCommandValidator : AbstractValidator<ChangePetInfoCommand>
{
    public ChangePetInfoCommandValidator()
    {
        RuleFor(p => p.PetId).MustBeValueObject(i => PetId.Create(i));
        RuleFor(p => p.SpeciesId).MustBeValueObject(SpeciesId.Create);
        RuleFor(p => p.Description).MustBeValueObject(Description.Create);
        RuleFor(p => p.BreedId).MustBeValueObject(BreedId.Create);
        RuleFor(p => p.Color).MustBeValueObject(Color.Create);
        RuleFor(p => p.ShelterId).MustBeValueObject(PetShelterId.Create);
        RuleFor(p => p.BirthDate).MustBeValueObject(Date.Create);
        RuleFor(p => p.VolunteerId).MustBeValueObject(VolunteerId.Create);
        RuleForEach(p => p.Requisiteses).MustBeValueObject(r => Requisites.Create(r.Name, r.Desc, r.PaymentMethod));
    }
}
