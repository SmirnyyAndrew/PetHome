using FluentValidation;
using PetHome.Volunteers.Domain.PetManagment.PetEntity;
using PetHome.Volunteers.Domain.PetManagment.VolunteerEntity;

namespace PetHome.Volunteers.Application.Features.Write.PetManegment.SetMainPhoto;
public class SetMainPhotoCommandValidator:AbstractValidator<SetPetMainPhotoCommand>    
{
    public SetMainPhotoCommandValidator()
    {
        RuleFor(m => m.VolunteerId).MustBeValueObject(VolunteerId.Create);
        RuleFor(m => m.PetId).MustBeValueObject(PetId.Create);
        RuleFor(m => m.FileName).MustBeValueObject(MinioFileName.Create);
    }
}
