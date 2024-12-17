using FluentValidation;
using PetHome.Application.Validator;
using PetHome.Domain.Shared.Error;

namespace PetHome.Application.Features.Volunteers.PetManegment.DeletePetMediaFiles;
public class DeletePetMediaFileValidator:AbstractValidator<DeletePetMediaFilesCommand>
{ 
    public DeletePetMediaFileValidator()
    {
        RuleFor(d => d.DeletePetMediaFilesDto.BucketName)
            .Must(b => !string.IsNullOrWhiteSpace(b))
            .WithError(Errors.Validation("Название bucket"));

        RuleForEach(d=>d.DeletePetMediaFilesDto.FilesName)
            .Must(b => !string.IsNullOrWhiteSpace(b))
            .WithError(Errors.Validation("Имя файла"));
    }
}
