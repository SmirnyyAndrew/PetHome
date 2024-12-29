using FluentValidation;
using PetHome.Domain.Shared.Error;

namespace PetHome.Volunteers.Application.Features.Write.PetManegment.UploadPetMediaFiles;
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