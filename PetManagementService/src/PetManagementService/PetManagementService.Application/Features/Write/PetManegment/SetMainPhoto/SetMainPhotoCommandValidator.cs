using FluentValidation;
using PetHome.Core.Application.Validation.Validator;
using PetManagementService.Domain.PetManagment.PetEntity;
using PetManagementService.Domain.PetManagment.VolunteerEntity;

namespace PetManagementService.Application.Features.Write.PetManegment.SetMainPhoto;
public class SetMainPhotoCommandValidator:AbstractValidator<SetPetMainPhotoCommand>    
{
    public SetMainPhotoCommandValidator()
    {
        RuleFor(m => m.VolunteerId).MustBeValueObject(VolunteerId.Create);
        RuleFor(m => m.PetId).MustBeValueObject(PetId.Create);
        //TODO: добавить валидацию с Error, в будущем добавленнего в nuget
        //RuleFor(m => m.FileName).MustBeValueObject(MinioFileName.Create);
    }
}
