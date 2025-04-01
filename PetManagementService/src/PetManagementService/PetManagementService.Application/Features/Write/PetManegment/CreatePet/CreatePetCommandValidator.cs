using FluentValidation;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Core.ValueObjects.MainInfo;
using PetHome.Core.ValueObjects.PetManagment.Breed;
using PetHome.Core.ValueObjects.PetManagment.Extra;
using PetHome.Core.ValueObjects.PetManagment.Pet;
using PetHome.Core.ValueObjects.PetManagment.Species;
using PetHome.Core.ValueObjects.PetManagment.Volunteer;

namespace PetManagementService.Application.Features.Write.PetManegment.CreatePet;
public class CreatePetCommandValidator : AbstractValidator<CreatePetCommand>
{
    public CreatePetCommandValidator()
    {
        RuleFor(p => p.PetMainInfoDto.Name).MustBeValueObject(PetName.Create);
        RuleFor(p => p.PetMainInfoDto.SpeciesId).MustBeValueObject(SpeciesId.Create);
        RuleFor(p => p.PetMainInfoDto.Description).MustBeValueObject(Description.Create);
        RuleFor(p => p.PetMainInfoDto.BreedId).MustBeValueObject(BreedId.Create);
        RuleFor(p => p.PetMainInfoDto.Color).MustBeValueObject(Color.Create);
        RuleFor(p => p.PetMainInfoDto.ShelterId).MustBeValueObject(PetShelterId.Create);
        RuleFor(p => p.PetMainInfoDto.BirthDate).MustBeValueObject(Date.Create);

        RuleForEach(p => p.PetMainInfoDto.Requisites)
            .MustBeValueObject(r => Requisites.Create(r.Name, r.Desc, r.PaymentMethod));

        RuleFor(p => p.VolunteerId).MustBeValueObject(VolunteerId.Create);
        RuleFor(p => p.PetMainInfoDto.ProfileCreateDate).MustBeValueObject(Date.Create);
    }
}
