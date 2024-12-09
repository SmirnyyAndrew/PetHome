using CSharpFunctionalExtensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using PetHome.Application.Interfaces;
using PetHome.Application.Interfaces.RepositoryInterfaces;
using PetHome.Domain.PetManagment.GeneralValueObjects;
using PetHome.Domain.PetManagment.PetEntity;
using PetHome.Domain.PetManagment.VolunteerEntity;
using PetHome.Domain.Shared.Error;

namespace PetHome.Application.Features.Volunteers.UploadPetMediaFilesVolunteer;
public class UploadPetMediaFilesVolunteerUseCase
{
    private readonly int MAX_STREAMS_LENGHT = 5;

    private IFilesProvider _filesProvider;
    private IVolunteerRepository _volunteerRepository;
    private ILogger<UploadPetMediaFilesVolunteerUseCase> _logger;

    public UploadPetMediaFilesVolunteerUseCase(
        IVolunteerRepository volunteerRepository,
        ILogger<UploadPetMediaFilesVolunteerUseCase> logger)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
    }




    public async Task<Result<string, Error>> Execute(
        IFilesProvider filesProvider,
        UploadPetMediaFilesVolunteerRequest uploadPetMediaRequest,
        Guid volunteerId,
        CancellationToken ct)
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


        IReadOnlyList<Media> oldPetMedias = pet.MediaDetails.Values.ToList();
        IReadOnlyList<Media> uploadPetMedias = uploadResult.Value;

        pet.UploadMedia(oldPetMedias, uploadPetMedias);

        await _volunteerRepository.Update(volunteer, ct);

        string message = $"В bucket {uploadPetMediaRequest.UploadPetMediaDto.BucketName} для pet {pet.Id} " +
            $"у volunteer {volunteer.Id} добавлены следующие файлы:\n " +
            $"{String.Join("\n", uploadResult.Value.Select(x => x.FileName))}";
        return message;
    }
}
