using CSharpFunctionalExtensions;
using FilesService.Core.Dto.File;
using FilesService.Core.Request.Minio;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetHome.Core.Infrastructure.Database;
using PetHome.Core.Infrastructure.MessageBus;
using PetHome.Core.Web.Extentions.ErrorExtentions;
using PetHome.SharedKernel.Responses.ErrorManagement;
using PetManagementService.Application.Database;
using PetManagementService.Domain.PetManagment.PetEntity;
using PetManagementService.Domain.PetManagment.VolunteerEntity;

namespace PetManagementService.Application.Features.Write.PetManegment.MinioUploadPetMediaFiles;
public class UploadPetMediaFilesUseCase
    : ICommandHandler<string, UploadPetMediaFilesCommand>
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
        UploadPetMediaFilesCommand command,
        CancellationToken ct)
    {
        var validationResult = await _validator.ValidateAsync(command, ct);
        if (validationResult.IsValid is false)
            return validationResult.Errors.ToErrorList();

        var transaction = await _unitOfWork.BeginTransaction(ct);

        var volunteerResult = await _volunteerRepository.GetById(command.VolunteerId, ct);
        if (volunteerResult.IsFailure)
            return Errors.NotFound($"Волонтёр с id {command.VolunteerId} не найден").ToErrorList();

        Volunteer volunteer = volunteerResult.Value;
        Pet pet = volunteer.Pets
            .FirstOrDefault(x => x.Id == command.UploadPetMediaDto.PetId);
        if (pet == null)
            return Errors.NotFound($"Питомец с id {command.UploadPetMediaDto.PetId} не найден").ToErrorList();


        List<MinioFileName> initedMinioFileNames = command.FileNames
            .Select(n => command.FilesHttpClient.InitName(n).Result)
            .ToList();
        MinioFilesInfoDto minioFilesInfoDto = new MinioFilesInfoDto(
            command.UploadPetMediaDto.BucketName,
            initedMinioFileNames);
        UploadFilesRequest uploadFileRequest = new UploadFilesRequest(
            command.Streams,
            minioFilesInfoDto,
            command.UploadPetMediaDto.CreateBucketIfNotExist);

        try
        {
            var uploadResult = await command.FilesHttpClient.UploadFiles(
                uploadFileRequest,
                ct);

            IReadOnlyList<MediaFile> uploadPetMedias = uploadResult.Value;
            pet.UploadMedia(uploadPetMedias);
        }
        catch (Exception ex)
        {
            MinioFilesInfoDto minioFileInfoDto = new MinioFilesInfoDto(
                   command.UploadPetMediaDto.BucketName,
                   initedMinioFileNames);
            await _messageQueue.WriteAsync(minioFileInfoDto, ct);

            return Errors.Conflict(ex.Message).ToErrorList();
        }

        await _volunteerRepository.Update(volunteer, ct);
        await _unitOfWork.SaveChanges(ct);
        transaction.Commit();

        string message = $"В bucket {command.UploadPetMediaDto.BucketName} для pet {pet.Id} " +
            $"у volunteer {volunteer.Id} добавлены следующие файлы:\n " +
            $"{string.Join("\n", command.FileNames)}";
        _logger.LogInformation(message);
        return message;
    }
}
