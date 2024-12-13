using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using PetHome.Application.Database;
using PetHome.Application.Interfaces;
using PetHome.Application.Interfaces.RepositoryInterfaces;
using PetHome.Domain.PetManagment.GeneralValueObjects;
using PetHome.Domain.PetManagment.PetEntity;
using PetHome.Domain.PetManagment.VolunteerEntity;
using PetHome.Domain.Shared.Error;
using PetHome.Infrastructure.Providers.Minio;
using System.Linq;

namespace PetHome.Application.Features.Volunteers.PetManegment.DeletePetMediaFiles;
public class DeletePetMediaFilesUseCase
{
    private IVolunteerRepository _volunteerRepository;
    private ILogger<DeletePetMediaFilesUseCase> _logger;
    private IUnitOfWork _unitOfWork;


    public DeletePetMediaFilesUseCase(
          IVolunteerRepository volunteerRepository,
          ILogger<DeletePetMediaFilesUseCase> logger,
          IUnitOfWork unitOfWork)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<string, Error>> Execute(
        IFilesProvider filesProvider,
        DeletePetMediaFilesRequest deleteMediaRequest,
        CancellationToken ct)
    {

        var transaction = await _unitOfWork.BeginTransaction(ct);
        try
        {
            var getVolunteerResult = await _volunteerRepository.GetById(deleteMediaRequest.VolunteerId, ct);
            if (getVolunteerResult.IsFailure)
                return Errors.NotFound($"Волонтёр с id {deleteMediaRequest.VolunteerId}");

            Volunteer volunteer = getVolunteerResult.Value;
            Pet? pet = volunteer.Pets.Where(x => x.Id == deleteMediaRequest.DeletePetMediaFilesDto.PetId).FirstOrDefault();
            if (pet == null)
                return Errors.NotFound($"Питомец с id {deleteMediaRequest.DeletePetMediaFilesDto.PetId}");

            List<string> oldFileNames = pet.Medias.Values.Select(x => x.FileName).ToList();
            List<Media> mediasToDelete = deleteMediaRequest.DeletePetMediaFilesDto.FilesName
                .Intersect(oldFileNames)
                .Select(m => Media.Create(deleteMediaRequest.DeletePetMediaFilesDto.BucketName, m).Value).ToList();
            pet.RemoveMedia(mediasToDelete);

            await _volunteerRepository.Update(volunteer, ct);


            MinioFileInfoDto minioFileInfoDto = new MinioFileInfoDto(
                deleteMediaRequest.DeletePetMediaFilesDto.BucketName,
                mediasToDelete.Select(x => x.FileName));

            var deleteResult = await filesProvider.DeleteFile(minioFileInfoDto, ct);
            if (deleteResult.IsFailure)
                return deleteResult.Error;

            await _unitOfWork.SaveChages(ct);
            transaction.Commit();

            string message = $"Из minio и pet удалены следующие файлы \n\t{string.Join("\n\r", mediasToDelete.Select(x => x.FileName))}";
            _logger.LogInformation(message);
            return message;
        }
        catch (Exception)
        {
            transaction.Rollback();
            _logger.LogInformation("Не удалось удалить медиаданные питомца {0}", deleteMediaRequest.DeletePetMediaFilesDto.PetId);
            return Errors.Failure("Database.is.failed");
        }
    }
}
