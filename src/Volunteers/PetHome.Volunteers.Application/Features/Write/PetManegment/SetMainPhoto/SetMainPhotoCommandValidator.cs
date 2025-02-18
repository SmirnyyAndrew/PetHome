using FluentValidation;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Core.ValueObjects.PetManagment.Pet;
using PetHome.Core.ValueObjects.PetManagment.Volunteer;

namespace PetHome.Volunteers.Application.Features.Write.PetManegment.SetMainPhoto;
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
