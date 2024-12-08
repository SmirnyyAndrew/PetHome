using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetHome.Application.Interfaces;
using PetHome.Application.Interfaces.RepositoryInterfaces;
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

        Pet pet = volunteerResult.Value.Pets
            .FirstOrDefault(x => x.Id == uploadPetMediaRequest.UploadPetMediaDto.PetId);
        if (pet == null)
            return Errors.NotFound($"Питомец с id {uploadPetMediaRequest.UploadPetMediaDto.PetId} не найден");

        var result = await filesProvider.UploadFile(
                uploadPetMediaRequest.Streams,
                uploadPetMediaRequest.UploadPetMediaDto.BucketName,
                uploadPetMediaRequest.FileNames,
                uploadPetMediaRequest.UploadPetMediaDto.CreateBucketIfNotExist,
                ct);
        if (result.IsFailure)
            return result.Error;

        pet.UploadMedia(result.Value);
        await _volunteerRepository.Update(volunteerResult.Value, ct);

        string message = $"В bucket {uploadPetMediaRequest.UploadPetMediaDto.BucketName} для pet {pet.Id} " +
            $"у volunteer {volunteerResult.Value.Id} добавлены следующие файлы:\n {String.Join("\n", result.Value.Select(x => x.FileName))}";
        return message;
    }
}
