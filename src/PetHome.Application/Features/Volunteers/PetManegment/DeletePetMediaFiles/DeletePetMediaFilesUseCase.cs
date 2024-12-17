using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetHome.Application.Database;
using PetHome.Application.Interfaces;
using PetHome.Application.Interfaces.RepositoryInterfaces;
using PetHome.Domain.PetManagment.GeneralValueObjects;
using PetHome.Domain.PetManagment.PetEntity;
using PetHome.Domain.PetManagment.VolunteerEntity;
using PetHome.Domain.Shared.Error;

namespace PetHome.Application.Features.Volunteers.PetManegment.DeletePetMediaFiles;
public class DeletePetMediaFilesUseCase
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
        IFilesProvider filesProvider,
        DeletePetMediaFilesCommand deleteMediaCommand,
        CancellationToken ct)
    {
        var validationResult = await _validator.ValidateAsync(deleteMediaCommand, ct);
        if (validationResult.IsValid is false)
            return (ErrorList)validationResult.Errors;

        var transaction = await _unitOfWork.BeginTransaction(ct);
        try
        {
            var getVolunteerResult = await _volunteerRepository.GetById(deleteMediaCommand.VolunteerId, ct);
            if (getVolunteerResult.IsFailure)
                return (ErrorList)Errors.NotFound($"Волонтёр с id {deleteMediaCommand.VolunteerId}");

            Volunteer volunteer = getVolunteerResult.Value;
            Pet? pet = volunteer.Pets.Where(x => x.Id == deleteMediaCommand.DeletePetMediaFilesDto.PetId).FirstOrDefault();
            if (pet == null)
                return (ErrorList)Errors.NotFound($"Питомец с id {deleteMediaCommand.DeletePetMediaFilesDto.PetId}");

            List<string> oldFileNames = pet.Medias.Values.Select(x => x.FileName).ToList();
            List<Media> mediasToDelete = deleteMediaCommand.DeletePetMediaFilesDto.FilesName
                .Intersect(oldFileNames)
                .Select(m => Media.Create(deleteMediaCommand.DeletePetMediaFilesDto.BucketName, m).Value).ToList();
            pet.RemoveMedia(mediasToDelete);

            await _volunteerRepository.Update(volunteer, ct);

            List<MinioFileName> minioFileNames = mediasToDelete.Select(m => MinioFileName.Create(m.FileName).Value).ToList();
            MinioFilesInfoDto minioFileInfoDto = new MinioFilesInfoDto(
                deleteMediaCommand.DeletePetMediaFilesDto.BucketName,
                minioFileNames);

            var deleteResult = await filesProvider.DeleteFile(minioFileInfoDto, ct);
            if (deleteResult.IsFailure)
                return (ErrorList)deleteResult.Error;

            await _unitOfWork.SaveChages(ct);
            transaction.Commit();

            string message = $"Из minio и pet удалены следующие файлы \n\t{string.Join("\n\r", mediasToDelete.Select(x => x.FileName))}";
            _logger.LogInformation(message);
            return message;
        }
        catch (Exception)
        {
            transaction.Rollback();
            _logger.LogInformation("Не удалось удалить медиаданные питомца {0}", deleteMediaCommand.DeletePetMediaFilesDto.PetId);
            return (ErrorList)Errors.Failure("Database.is.failed");
        }
    }
}
