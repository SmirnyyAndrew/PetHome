using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetHome.Core.Extentions.ErrorExtentions;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Core.ValueObjects;
using PetHome.Framework.Database;
using PetHome.Volunteers.Application.Database;
using PetHome.Volunteers.Domain.PetManagment.PetEntity;
using PetHome.Volunteers.Domain.PetManagment.VolunteerEntity;

namespace PetHome.Volunteers.Application.Features.Write.PetManegment.DeletePetMediaFiles;
public class DeletePetMediaFilesUseCase
    : ICommandHandler<string, DeletePetMediaFilesCommand>
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<DeletePetMediaFilesUseCase> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<DeletePetMediaFilesCommand> _validator;

    public DeletePetMediaFilesUseCase(
          IVolunteerRepository volunteerRepository,
          ILogger<DeletePetMediaFilesUseCase> logger,
          IUnitOfWork unitOfWork,
          IValidator<DeletePetMediaFilesCommand> validator)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<Result<string, ErrorList>> Execute(
        DeletePetMediaFilesCommand command,
        CancellationToken ct)
    {
        var validationResult = await _validator.ValidateAsync(command, ct);
        if (validationResult.IsValid is false)
            return validationResult.Errors.ToErrorList();

        var transaction = await _unitOfWork.BeginTransaction(ct);
        try
        {
            var getVolunteerResult = await _volunteerRepository.GetById(command.VolunteerId, ct);
            if (getVolunteerResult.IsFailure)
                return Errors.NotFound($"Волонтёр с id {command.VolunteerId}").ToErrorList();

            Volunteer volunteer = getVolunteerResult.Value;
            Pet? pet = volunteer.Pets.Where(x => x.Id == command.DeletePetMediaFilesDto.PetId)
                .FirstOrDefault();
            if (pet == null)
                return Errors.NotFound($"Питомец с id {command.DeletePetMediaFilesDto.PetId}").ToErrorList();

            List<string> oldFileNames = pet.Medias.Values.Select(x => x.FileName).ToList();
            List<Media> mediasToDelete = command.DeletePetMediaFilesDto.FilesName
                .Intersect(oldFileNames)
                .Select(m => Media.Create(command.DeletePetMediaFilesDto.BucketName, m).Value)
                .ToList();
            pet.RemoveMedia(mediasToDelete);

            await _volunteerRepository.Update(volunteer, ct);

            List<MinioFileName> minioFileNames = mediasToDelete
                .Select(m => MinioFileName.Create(m.FileName).Value)
                .ToList();
            MinioFilesInfoDto minioFileInfoDto = new MinioFilesInfoDto(
                command.DeletePetMediaFilesDto.BucketName,
                minioFileNames);

            var deleteResult = await command.FileProvider.DeleteFile(minioFileInfoDto, ct);
            if (deleteResult.IsFailure)
                return deleteResult.Error.ToErrorList();

            await _unitOfWork.SaveChages(ct);
            transaction.Commit();

            string message = $"Из minio и pet удалены следующие файлы \n\t{string.Join("\n\r", mediasToDelete.Select(x => x.FileName))}";
            _logger.LogInformation(message);
            return message;
        }
        catch (Exception)
        {
            transaction.Rollback();
            _logger.LogInformation("Не удалось удалить медиаданные питомца {0}", command.DeletePetMediaFilesDto.PetId);
            return Errors.Failure("Database.is.failed").ToErrorList();
        }
    }
}
