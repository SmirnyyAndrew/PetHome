using FluentValidation;
using PetHome.Application.Validator;
using PetHome.Domain.PetManagment.PetEntity;
using PetHome.Domain.PetManagment.VolunteerEntity;

namespace PetHome.Application.Features.Write.PetManegment.SetMainPhoto;
public class SetMainPhotoCommandValidator:AbstractValidator<SetPetMainPhotoCommand>    
{
    public SetMainPhotoCommandValidator()
    {
        RuleFor(m => m.VolunteerId).MustBeValueObject(VolunteerId.Create);
        RuleFor(m => m.PetId).MustBeValueObject(PetId.Create);
        RuleFor(m => m.FileName).MustBeValueObject(MinioFileName.Create);
    }
}
