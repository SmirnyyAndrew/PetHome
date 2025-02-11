using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetHome.Core.Constants;
using PetHome.Core.Extentions.ErrorExtentions;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.Response.Messaging;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Core.ValueObjects.File;
using PetHome.Core.ValueObjects.PetManagment.Extra;
using PetHome.Framework.Database;
using PetHome.Volunteers.Application.Database;
using PetHome.Volunteers.Domain.PetManagment.PetEntity;
using PetHome.Volunteers.Domain.PetManagment.VolunteerEntity;

namespace PetHome.Volunteers.Application.Features.Write.PetManegment.UploadPetMediaFiles;
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
       [FromKeyedServices(Constants.VOLUNTEER_UNIT_OF_WORK_KEY)] IUnitOfWork unitOfWork,
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
            .Select(n => command.FilesProvider.InitName(n))
            .ToList();
        MinioFilesInfoDto minioFilesInfoDto = new MinioFilesInfoDto(
            command.UploadPetMediaDto.BucketName,
            initedMinioFileNames);
        var uploadResult = await command.FilesProvider.UploadFile(
            command.Streams,
            minioFilesInfoDto,
            command.UploadPetMediaDto.CreateBucketIfNotExist,
            ct);

        if (uploadResult.IsFailure)
        {
            MinioFilesInfoDto minioFileInfoDto = new MinioFilesInfoDto(
                command.UploadPetMediaDto.BucketName,
                initedMinioFileNames);
            await _messageQueue.WriteAsync(minioFileInfoDto, ct);
            return uploadResult.Error.ToErrorList();
        }

        IReadOnlyList<MediaFile> uploadPetMedias = uploadResult.Value;

        pet.UploadMedia(uploadPetMedias);

        await _volunteerRepository.Update(volunteer, ct);
        await _unitOfWork.SaveChanges(ct);
        transaction.Commit();

        string message = $"В bucket {command.UploadPetMediaDto.BucketName} для pet {pet.Id} " +
            $"у volunteer {volunteer.Id} добавлены следующие файлы:\n " +
            $"{string.Join("\n", uploadResult.Value.Select(x => x.FileName))}";
        _logger.LogInformation(message);
        return message;
    }
}
