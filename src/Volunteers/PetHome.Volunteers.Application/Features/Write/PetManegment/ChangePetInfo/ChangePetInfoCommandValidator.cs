using FluentValidation;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Core.ValueObjects;
using PetHome.Species.Domain.SpeciesManagment.BreedEntity;
using PetHome.Species.Domain.SpeciesManagment.SpeciesEntity;
using PetHome.Volunteers.Domain.PetManagment.PetEntity;
using PetHome.Volunteers.Domain.PetManagment.VolunteerEntity;

namespace PetHome.Volunteers.Application.Features.Write.PetManegment.ChangePetInfo;
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
