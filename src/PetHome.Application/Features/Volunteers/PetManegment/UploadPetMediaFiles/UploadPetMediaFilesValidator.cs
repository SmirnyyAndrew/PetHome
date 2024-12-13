using FluentValidation;
using PetHome.Application.Features.Volunteers.PetManegment.UploadPetMediaFilesVolunteer;
using PetHome.Application.Validator;
using PetHome.Domain.Shared.Error;

namespace PetHome.Application.Features.Volunteers.PetManegment.UploadPetMediaFiles;
public class UploadPetMediaFilesValidator : AbstractValidator<UploadPetMediaFilesRequest>
{
    public UploadPetMediaFilesValidator()
    {
        RuleFor(d => d.UploadPetMediaDto.BucketName)
            .Must(b => !string.IsNullOrWhiteSpace(b))
            .WithError(Errors.Validation("Название bucket"));

        RuleForEach(d => d.FileNames)
            .Must(f => !string.IsNullOrWhiteSpace(f))
            .WithError(Errors.Validation("Имя файла"));
    }
}