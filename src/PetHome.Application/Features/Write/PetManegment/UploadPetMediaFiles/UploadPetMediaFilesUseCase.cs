using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetHome.Application.Database;
using PetHome.Application.Interfaces;
using PetHome.Application.Interfaces.RepositoryInterfaces;
using PetHome.Application.Messaging;
using PetHome.Application.Validator;
using PetHome.Domain.PetManagment.GeneralValueObjects;
using PetHome.Domain.PetManagment.PetEntity;
using PetHome.Domain.PetManagment.VolunteerEntity;
using PetHome.Domain.Shared.Error;

namespace PetHome.Application.Features.Write.PetManegment.UploadPetMediaFiles;
public class UploadPetMediaFilesUseCase 
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<UploadPetMediaFilesUseCase> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessageQueue _messageQueue;
    private readonly IValidator<UploadPetMediaFilesCommand> _validator;

    public UploadPetMediaFilesUseCase(
        IVolunteerRepository volunteerRepository,
        ILogger<UploadPetMediaFilesUseCase> logger,
        IUnitOfWork unitOfWork,
        IMessageQueue messageQueue,
        IValidator<UploadPetMediaFilesCommand> validator)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _messageQueue = messageQueue;
        _validator = validator;
    }

    public async Task<Result<string, ErrorList>> Execute(
        IFilesProvider filesProvider,
        UploadPetMediaFilesCommand uploadPetMediaCommand,
        Guid volunteerId,
        CancellationToken ct)
    {
        var validationResult = await _validator.ValidateAsync(uploadPetMediaCommand, ct);
        if (validationResult.IsValid is false)
            return (ErrorList)validationResult.Errors;

        var transaction = await _unitOfWork.BeginTransaction(ct);
        try
        {
            var volunteerResult = await _volunteerRepository.GetById(volunteerId, ct);
            if (volunteerResult.IsFailure)
                return (ErrorList)Errors.NotFound($"Волонтёр с id {volunteerId} не найден");

            Volunteer volunteer = volunteerResult.Value;
            Pet pet = volunteer.Pets
                .FirstOrDefault(x => x.Id == uploadPetMediaCommand.UploadPetMediaDto.PetId);
            if (pet == null)
                return (ErrorList)Errors.NotFound($"Питомец с id {uploadPetMediaCommand.UploadPetMediaDto.PetId} не найден");


            List<MinioFileName> initedMinioFileNames = uploadPetMediaCommand.FileNames
                .Select(n => filesProvider.InitName(n))
                .ToList();
            MinioFilesInfoDto minioFilesInfoDto = new MinioFilesInfoDto(
                uploadPetMediaCommand.UploadPetMediaDto.BucketName,
                initedMinioFileNames);
            var uploadResult = await filesProvider.UploadFile(
                uploadPetMediaCommand.Streams,
                minioFilesInfoDto,
                uploadPetMediaCommand.UploadPetMediaDto.CreateBucketIfNotExist,
                ct);

            if (uploadResult.IsFailure)
            {
                MinioFilesInfoDto minioFileInfoDto = new MinioFilesInfoDto(
                    uploadPetMediaCommand.UploadPetMediaDto.BucketName,
                    initedMinioFileNames);
                await _messageQueue.WriteAsync(minioFileInfoDto, ct);
                return (ErrorList)uploadResult.Error;
            }

            IReadOnlyList<Media> uploadPetMedias = uploadResult.Value;

            pet.UploadMedia(uploadPetMedias);

            await _volunteerRepository.Update(volunteer, ct);

            await _unitOfWork.SaveChages(ct);
            transaction.Commit();

            string message = $"В bucket {uploadPetMediaCommand.UploadPetMediaDto.BucketName} для pet {pet.Id} " +
                $"у volunteer {volunteer.Id} добавлены следующие файлы:\n " +
                $"{string.Join("\n", uploadResult.Value.Select(x => x.FileName))}";
            _logger.LogInformation(message);
            return message;
        }
        catch (Exception)
        {
            transaction.Rollback();
            _logger.LogInformation("Не удалось создать медиаданные питомца");
            return (ErrorList)Errors.Failure("Database.is.failed");
        }
    }
}
