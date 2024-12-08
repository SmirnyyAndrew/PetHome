using FluentValidation;
using PetHome.Application.Validator;
using PetHome.Domain.PetManagment.GeneralValueObjects;
using PetHome.Domain.PetManagment.PetEntity;
using PetHome.Domain.PetManagment.VolunteerEntity;
using PetHome.Domain.Shared.Error;

namespace PetHome.Application.Features.Volunteers.CreatePetVolunteer;
public class VolunteerCreatePetRequestValidator : AbstractValidator<VolunteerCreatePetRequest>
{
    public VolunteerCreatePetRequestValidator()
    {
        RuleFor(p => p.MainInfoDto.Name).MustBeValueObject(PetName.Create);
        RuleFor(p => p.MainInfoDto.SpeciesId).MustBeValueObject(SpeciesId.Create);
        RuleFor(p => p.MainInfoDto.Description).MustBeValueObject(Description.Create);
        RuleFor(p => p.MainInfoDto.BreedId).MustBeValueObject(BreedId.Create);
        RuleFor(p => p.MainInfoDto.Color).MustBeValueObject(Color.Create);
        RuleFor(p => p.MainInfoDto.ShelterId).MustBeValueObject(PetShelterId.Create);
        RuleFor(p => p.MainInfoDto.BirthDate).MustBeValueObject(Date.Create);

        RuleForEach(p => p.MainInfoDto.Requisites)
            .MustBeValueObject(r => Requisites.Create(r.Name, r.Desc, r.PaymentMethod));

        RuleForEach(p => p.PhotosDto)
            .Must(dto => !string.IsNullOrWhiteSpace(dto.BucketName)
                      && !string.IsNullOrWhiteSpace(dto.FileName))
            .WithError(Errors.Validation("Название bucket и файла"));

        RuleFor(p => p.VolunteerId).MustBeValueObject(VolunteerId.Create);
        RuleFor(p => p.MainInfoDto.ProfileCreateDate).MustBeValueObject(Date.Create);
    }
}
