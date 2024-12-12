using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetHome.Application.Database;
using PetHome.Application.Interfaces;
using PetHome.Application.Interfaces.RepositoryInterfaces;
using PetHome.Domain.PetManagment.GeneralValueObjects;
using PetHome.Domain.PetManagment.PetEntity;
using PetHome.Domain.PetManagment.VolunteerEntity;
using PetHome.Domain.Shared.Error;

namespace PetHome.Application.Features.Volunteers.PetManegment.UploadPetMediaFilesVolunteer;
public class UploadPetMediaFilesUseCase
{
    private IVolunteerRepository _volunteerRepository;
    private ILogger<UploadPetMediaFilesUseCase> _logger;
    private IUnitOfWork _unitOfWork;

    public UploadPetMediaFilesUseCase(
        IVolunteerRepository volunteerRepository,
        ILogger<UploadPetMediaFilesUseCase> logger,
        IUnitOfWork unitOfWork)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<string, Error>> Execute(
        IFilesProvider filesProvider,
        UploadPetMediaFilesRequest uploadPetMediaRequest,
        Guid volunteerId,
        CancellationToken ct)
    {
        var transaction = await _unitOfWork.BeginTransaction(ct);
        try
        {
            var volunteerResult = await _volunteerRepository.GetById(volunteerId, ct);
            if (volunteerResult.IsFailure)
                return Errors.NotFound($"Волонтёр с id {volunteerId} не найден");

            Volunteer volunteer = volunteerResult.Value;
            Pet pet = volunteer.Pets
                .FirstOrDefault(x => x.Id == uploadPetMediaRequest.UploadPetMediaDto.PetId);
            if (pet == null)
                return Errors.NotFound($"Питомец с id {uploadPetMediaRequest.UploadPetMediaDto.PetId} не найден");


            var uploadResult = await filesProvider.UploadFile(
                    uploadPetMediaRequest.Streams,
                    uploadPetMediaRequest.UploadPetMediaDto.BucketName,
                    uploadPetMediaRequest.FileNames,
                    uploadPetMediaRequest.UploadPetMediaDto.CreateBucketIfNotExist,
                    ct);
            if (uploadResult.IsFailure)
                return uploadResult.Error;


            IReadOnlyList<Media> uploadPetMedias = uploadResult.Value;

            pet.UploadMedia(uploadPetMedias);

            await _volunteerRepository.Update(volunteer, ct);

            await _unitOfWork.SaveChages(ct);
            transaction.Commit();

            string message = $"В bucket {uploadPetMediaRequest.UploadPetMediaDto.BucketName} для pet {pet.Id} " +
                $"у volunteer {volunteer.Id} добавлены следующие файлы:\n " +
                $"{string.Join("\n", uploadResult.Value.Select(x => x.FileName))}";
            _logger.LogInformation(message);
            return message;
        }
        catch (Exception)
        {
            transaction.Rollback();
            _logger.LogInformation($"Не удалось создать медиаданные питомца");
            return Errors.Failure("Database.is.failed");
        }
    }
}
