using FluentValidation;
using PetHome.Application.Validator;
using PetHome.Domain.Shared.Error;

namespace PetHome.Application.Features.Write.Volunteers.PetManegment.UploadPetMediaFiles;
public class UploadPetMediaFilesCommandValidator : AbstractValidator<UploadPetMediaFilesCommand>
{
    public UploadPetMediaFilesCommandValidator()
    {
        RuleFor(d => d.UploadPetMediaDto.BucketName)
            .Must(b => !string.IsNullOrWhiteSpace(b))
            .WithError(Errors.Validation("Название bucket"));

        RuleForEach(d => d.FileNames)
            .Must(f => !string.IsNullOrWhiteSpace(f))
            .WithError(Errors.Validation("Имя файла"));
    }
}